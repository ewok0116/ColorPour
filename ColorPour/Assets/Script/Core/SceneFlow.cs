using UnityEngine.SceneManagement;

namespace BottleFill.Core
{
    /// <summary>
    /// Sahne gecislerini tek yerden yonetir ve Main'de secilen leveli
    /// Level sahnesine tasir.
    /// </summary>
    public static class SceneFlow
    {
        public const string MainScene = "Main";
        public const string LevelScene = "Level";

        /// <summary>Level sahnesini dogrudan Play'e basarak acarsan 0 olur, yani ilk level.</summary>
        public static int SelectedLevelIndex { get; private set; }

        public static void LoadLevel(int index)
        {
            SelectedLevelIndex = index;
            SceneManager.LoadScene(LevelScene);
        }

        public static void LoadMain() => SceneManager.LoadScene(MainScene);
    }
}
