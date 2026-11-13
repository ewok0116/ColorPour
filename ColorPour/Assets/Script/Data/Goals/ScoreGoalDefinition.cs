using BottleFill.Levels;
using UnityEngine;

namespace BottleFill.Data.Goals
{
    [CreateAssetMenu(fileName = "Goal_Score_", menuName = "Bottle Fill/Goals/Score Goal")]
    public class ScoreGoalDefinition : GoalDefinition
    {
        [Min(1)] [SerializeField] private int targetScore = 500;

        public override GoalData ToData() => new GoalData(GoalTypes.Score, targetScore);
    }
}
