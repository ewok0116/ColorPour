using System.Collections.Generic;

namespace BottleFill.Model.Goals
{
    /// <summary>
    /// Bir hedefin ilerlemesini olcmek icin ihtiyac duyabilecegi oyun verileri.
    /// Yeni bir hedef turu yeni veri isterse buraya eklenir.
    /// </summary>
    public class GoalContext
    {
        public ScoreModel Score { get; }
        public IReadOnlyList<BottleModel> Bottles { get; }

        public GoalContext(ScoreModel score, IReadOnlyList<BottleModel> bottles)
        {
            Score = score;
            Bottles = bottles;
        }
    }
}
