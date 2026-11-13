using System;

namespace BottleFill.Meta
{
    /// <summary>Hangi levellarin acik oldugunu tutar.</summary>
    public class LevelProgress
    {
        public int HighestUnlocked { get; private set; }
        public event Action Changed;

        public LevelProgress(int highestUnlocked) => HighestUnlocked = Math.Max(0, highestUnlocked);

        public bool IsUnlocked(int levelIndex) => levelIndex <= HighestUnlocked;

        /// <summary>Leveli tamamlandi isaretler. Ilk kez tamamlandiysa true doner.</summary>
        public bool MarkCompleted(int levelIndex)
        {
            if (levelIndex < HighestUnlocked) return false;

            HighestUnlocked = levelIndex + 1;
            Changed?.Invoke();
            return true;
        }
    }
}
