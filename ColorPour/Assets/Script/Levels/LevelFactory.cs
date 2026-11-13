using System.Collections.Generic;
using BottleFill.Data;
using BottleFill.Model;
using BottleFill.Model.Goals;

namespace BottleFill.Levels
{
    /// <summary>
    /// LevelData (duz veri) + ColorRegistry -> calisan Level.
    /// Veri nereden gelirse gelsin (asset, JSON, sunucu) oyun levelini burada kurar.
    /// </summary>
    public class LevelFactory
    {
        // Her muslugun farkli ama tekrarlanabilir bir sira uretmesi icin seed'e eklenen carpan.
        private const int DispenserSeedStep = 7919;

        private readonly ColorRegistry colors;
        private readonly ScoringConfig scoring;

        public LevelFactory(ColorRegistry colors, ScoringConfig scoring)
        {
            this.colors = colors;
            this.scoring = scoring;
        }

        public Level Build(LevelData data)
        {
            var bottles = new List<BottleModel>();
            foreach (var bottleData in data.bottles)
            {
                var layers = new List<Layer>();
                foreach (var layerData in bottleData.layers)
                    layers.Add(new Layer(colors.Get(layerData.color), layerData.frozen));

                bottles.Add(new BottleModel(bottleData.capacity, layers));
            }

            var dispensers = BuildDispensers(data);

            var score = new ScoreModel(scoring);
            var context = new GoalContext(score, bottles);

            var trackers = new List<IGoalTracker>();
            foreach (var goalData in data.goals)
                trackers.Add(GoalFactory.Create(goalData, context));

            return new Level(data.id, bottles, dispensers, score, new LevelGoals(trackers));
        }

        private List<DropSequence> BuildDispensers(LevelData data)
        {
            var defaultPool = ResolveColors(data.dropColors);
            var result = new List<DropSequence>();

            // Musluk tanimlanmamis levellar icin tek musluk.
            if (data.dispensers.Count == 0)
            {
                result.Add(new DropSequence(defaultPool, data.seed));
                return result;
            }

            for (int i = 0; i < data.dispensers.Count; i++)
            {
                var own = data.dispensers[i].colors;
                var pool = own.Count > 0 ? ResolveColors(own) : defaultPool;
                result.Add(new DropSequence(pool, data.seed + i * DispenserSeedStep));
            }
            return result;
        }

        private List<ColorDefinition> ResolveColors(List<string> ids)
        {
            var list = new List<ColorDefinition>();
            foreach (var id in ids) list.Add(colors.Get(id));
            return list;
        }
    }
}
