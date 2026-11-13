using BottleFill.Core.Save;
using BottleFill.Data;
using BottleFill.Levels;
using BottleFill.Meta.Lab;
using UnityEngine;

namespace BottleFill.Core
{
    /// <summary>
    /// Oyunun ortak ayarlari. Sahneler tek tek asset aramak yerine bunu kullanir.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Bottle Fill/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Leveller")]
        [SerializeField] private LevelCatalog levelCatalog;
        [SerializeField] private ColorRegistry colorRegistry;
        [SerializeField] private ColorMixTable mixTable;
        [SerializeField] private ScoringConfig scoring;

        [Header("Ekonomi")]
        [Tooltip("Level kendi odulunu belirtmiyorsa ilk tamamlanista verilecek para.")]
        [Min(0)] [SerializeField] private int defaultLevelReward = 50;

        [Header("Laboratuvar")]
        [SerializeField] private LabCatalog labCatalog;

        public LevelCatalog LevelCatalog => levelCatalog;
        public ColorRegistry ColorRegistry => colorRegistry;
        public ColorMixTable MixTable => mixTable;
        public ScoringConfig Scoring => scoring;
        public int DefaultLevelReward => defaultLevelReward;
        public LabCatalog LabCatalog => labCatalog;

        /// <summary>Levellerin nereden gelecegine karar verilen tek yer.</summary>
        public ILevelRepository CreateLevelRepository() => new CatalogLevelRepository(levelCatalog);

        /// <summary>Oyuncu verisinin nereye kaydedilecegine karar verilen tek yer.</summary>
        public ISaveStorage CreateSaveStorage() => new FileSaveStorage();
    }
}
