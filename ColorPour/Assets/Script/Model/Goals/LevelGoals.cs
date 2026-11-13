using System;
using System.Collections.Generic;
using BottleFill.Model.Reactions;

namespace BottleFill.Model.Goals
{
    /// <summary>
    /// Bir levelin tum hedeflerini bir arada tutar. Hepsi tamamlaninca
    /// Completed olayini bir kez tetikler.
    /// </summary>
    public class LevelGoals
    {
        private readonly List<IGoalTracker> trackers;
        private bool completedRaised;

        public IReadOnlyList<IGoalTracker> Trackers => trackers;
        public event Action Completed;

        public LevelGoals(List<IGoalTracker> trackers) => this.trackers = trackers;

        public bool AllCompleted
        {
            get
            {
                foreach (var t in trackers)
                    if (!t.IsCompleted) return false;
                return trackers.Count > 0;
            }
        }

        public void OnReactions(IReadOnlyList<ReactionEvent> events)
        {
            foreach (var t in trackers) t.OnReactions(events);

            if (!completedRaised && AllCompleted)
            {
                completedRaised = true;
                Completed?.Invoke();
            }
        }
    }
}
