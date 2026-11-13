using System;
using System.Collections.Generic;
using BottleFill.Model.Goals;

namespace BottleFill.Levels
{
    /// <summary>
    /// Duz hedef verisini (GoalData) calisan bir tracker'a cevirir.
    /// Yeni hedef turu eklemek: tracker sinifini yaz, GoalTypes'a sabit ekle,
    /// asagidaki sozluge bir satir ekle.
    /// </summary>
    public static class GoalFactory
    {
        private static readonly Dictionary<string, Func<GoalData, GoalContext, IGoalTracker>> creators =
            new Dictionary<string, Func<GoalData, GoalContext, IGoalTracker>>
            {
                [GoalTypes.Score]    = (data, ctx) => new ScoreGoalTracker(ctx.Score, data.target),
                [GoalTypes.BreakIce] = (data, ctx) => new BreakIceGoalTracker(ctx.Bottles),
            };

        public static IGoalTracker Create(GoalData data, GoalContext context)
        {
            if (data != null && creators.TryGetValue(data.type ?? "", out var create))
                return create(data, context);

            throw new ArgumentException($"Bilinmeyen hedef turu: '{data?.type}'");
        }
    }
}
