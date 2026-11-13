using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BottleFill.Core.Save;
using BottleFill.Meta;
using BottleFill.Meta.Lab;
using UnityEngine;

namespace BottleFill.Core
{
    /// <summary>
    /// Sahneler arasinda yasayan servisler: cuzdan, level ilerlemesi, laboratuvar.
    /// Ilk acilan sahne InitializeAsync ile baslatir; sonrakiler hazir olani kullanir.
    /// Her degisiklikte oyuncu verisi otomatik kaydedilir.
    /// </summary>
    public sealed class GameServices
    {
        private static GameServices instance;
        private static Task<GameServices> initialization;

        public static GameServices Instance =>
            instance ?? throw new InvalidOperationException("GameServices hazir degil. Once InitializeAsync cagrilmali.");

        public static Task<GameServices> InitializeAsync(GameConfig config) =>
            initialization ??= CreateAsync(config);

        // Editorde Play'e her basista temiz baslamak icin.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
            initialization = null;
        }

        private readonly ISaveStorage storage;

        public GameConfig Config { get; }
        public Wallet Wallet { get; }
        public LevelProgress Progress { get; }
        public LabModel Lab { get; }

        private GameServices(GameConfig config, ISaveStorage storage, PlayerData data)
        {
            Config = config;
            this.storage = storage;

            Wallet = new Wallet(data.coins);
            Progress = new LevelProgress(data.highestUnlockedLevel);
            Lab = new LabModel(config.LabCatalog, data.ownedLabUpgrades, Wallet, Progress);

            Wallet.Changed += _ => Save();
            Progress.Changed += Save;
            Lab.Purchased += _ => Save();
        }

        private static async Task<GameServices> CreateAsync(GameConfig config)
        {
            var storage = config.CreateSaveStorage();
            var data = await storage.LoadAsync();
            instance = new GameServices(config, storage, data);
            return instance;
        }

        /// <summary>Level bittiginde cagrilir. Ilk tamamlanissa odulu verir; kazanilan parayi doner.</summary>
        public int ReportLevelCompleted(int levelIndex, int reward)
        {
            if (!Progress.MarkCompleted(levelIndex)) return 0;

            Wallet.Add(reward);
            return reward;
        }

        private async void Save()
        {
            try
            {
                await storage.SaveAsync(new PlayerData
                {
                    coins = Wallet.Coins,
                    highestUnlockedLevel = Progress.HighestUnlocked,
                    ownedLabUpgrades = new List<string>(Lab.OwnedIds)
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Bottle Fill/Debug/+500 Para")]
        private static void DebugAddCoins()
        {
            if (instance == null)
            {
                Debug.LogWarning("Play modundayken calisir.");
                return;
            }
            instance.Wallet.Add(500);
        }

        [UnityEditor.MenuItem("Bottle Fill/Debug/Kaydi Sil")]
        private static void DebugDeleteSave()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Once Play modundan cik.");
                return;
            }
            FileSaveStorage.DeleteDefaultFile();
        }
#endif
    }
}
