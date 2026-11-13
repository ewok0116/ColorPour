using System.Collections.Generic;
using BottleFill.Model;
using BottleFill.Model.Goals;

namespace BottleFill.Levels
{
    /// <summary>
    /// Oynanmakta olan bir levelin calisan hali: siseler, musluklar,
    /// puan ve hedefler. LevelFactory tarafindan LevelData'dan uretilir.
    /// </summary>
    public class Level
    {
        public string Id { get; }
        public IReadOnlyList<BottleModel> Bottles { get; }

        /// <summary>Her musluk icin bir damla sirasi.</summary>
        public IReadOnlyList<DropSequence> Dispensers { get; }

        public ScoreModel Score { get; }
        public LevelGoals Goals { get; }

        public Level(string id, IReadOnlyList<BottleModel> bottles, IReadOnlyList<DropSequence> dispensers,
            ScoreModel score, LevelGoals goals)
        {
            Id = id;
            Bottles = bottles;
            Dispensers = dispensers;
            Score = score;
            Goals = goals;
        }
    }
}
