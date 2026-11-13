using BottleFill.Levels;
using UnityEngine;

namespace BottleFill.Data.Goals
{
    /// <summary>Leveldeki tum buzlu katmanlari kir. Hedef sayisi levelden otomatik bulunur.</summary>
    [CreateAssetMenu(fileName = "Goal_BreakIce", menuName = "Bottle Fill/Goals/Break Ice Goal")]
    public class BreakIceGoalDefinition : GoalDefinition
    {
        public override GoalData ToData() => new GoalData(GoalTypes.BreakIce);
    }
}
