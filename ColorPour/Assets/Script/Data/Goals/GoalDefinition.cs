using BottleFill.Levels;
using UnityEngine;

namespace BottleFill.Data.Goals
{
    /// <summary>
    /// Editorde hedef tasarlamak icin kullanilan asset. Kendini duz GoalData'ya
    /// cevirir; oyun sirasindaki tracker'i GoalFactory uretir.
    /// </summary>
    public abstract class GoalDefinition : ScriptableObject
    {
        public abstract GoalData ToData();
    }
}
