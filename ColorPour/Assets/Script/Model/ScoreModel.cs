using System;
using System.Collections.Generic;
using BottleFill.Data;
using BottleFill.Model.Reactions;

namespace BottleFill.Model
{
    /// <summary>
    /// Tepki olaylarini puana cevirir. UI bu sinifin Changed olayini dinler,
    /// puani kendisi hesaplamaz.
    /// </summary>
    public class ScoreModel
    {
        private readonly ScoringConfig config;

        public int Score { get; private set; }
        public event Action<int> Changed;

        public ScoreModel(ScoringConfig config) => this.config = config;

        public void Apply(IReadOnlyList<ReactionEvent> events)
        {
            int gained = 0;
            foreach (var e in events)
            {
                switch (e.Type)
                {
                    case ReactionType.Evaporated: gained += config.pointsPerEvaporate; break;
                    case ReactionType.Mixed:      gained += config.pointsPerMix; break;
                    case ReactionType.Exploded:   gained += config.pointsPerExplosion; break;
                    case ReactionType.IceBroken:  gained += config.pointsPerIceBreak; break;
                }
            }

            if (gained == 0) return;
            Score += gained;
            Changed?.Invoke(Score);
        }

        public void Reset()
        {
            Score = 0;
            Changed?.Invoke(Score);
        }
    }
}
