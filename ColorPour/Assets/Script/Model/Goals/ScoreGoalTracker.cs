using System;
using System.Collections.Generic;
using BottleFill.Model.Reactions;
using UnityEngine;

namespace BottleFill.Model.Goals
{
    public class ScoreGoalTracker : IGoalTracker
    {
        private readonly ScoreModel score;
        private readonly int target;

        public ScoreGoalTracker(ScoreModel score, int target)
        {
            this.score = score;
            this.target = Mathf.Max(1, target);
        }

        public bool IsCompleted => score.Score >= target;
        public float Progress => Mathf.Clamp01((float)score.Score / target);
        public string ProgressText => $"{Mathf.Min(score.Score, target)} / {target}";

        public event Action Changed;

        // Puan zaten ScoreModel'de hesaplaniyor; burada sadece degisikligi duyuruyoruz.
        public void OnReactions(IReadOnlyList<ReactionEvent> events) => Changed?.Invoke();
    }
}
