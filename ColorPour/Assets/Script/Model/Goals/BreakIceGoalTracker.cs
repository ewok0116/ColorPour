using System;
using System.Collections.Generic;
using BottleFill.Model.Reactions;
using UnityEngine;

namespace BottleFill.Model.Goals
{
    /// <summary>
    /// Levelin basindaki tum buzlarin kirilmasini bekler. Hedef sayisi
    /// level baslarken siselerdeki donmus katmanlar sayilarak bulunur.
    /// </summary>
    public class BreakIceGoalTracker : IGoalTracker
    {
        private readonly int total;
        private int broken;

        public BreakIceGoalTracker(IReadOnlyList<BottleModel> bottles)
        {
            foreach (var bottle in bottles) total += bottle.CountFrozen();
        }

        public bool IsCompleted => broken >= total;
        public float Progress => total == 0 ? 1f : Mathf.Clamp01((float)broken / total);
        public string ProgressText => $"{Mathf.Min(broken, total)} / {total}";

        public event Action Changed;

        public void OnReactions(IReadOnlyList<ReactionEvent> events)
        {
            int before = broken;
            foreach (var e in events)
                if (e.Type == ReactionType.IceBroken) broken++;

            if (broken != before) Changed?.Invoke();
        }
    }
}
