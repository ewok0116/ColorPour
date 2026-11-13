using System;
using System.Collections.Generic;
using BottleFill.Core;
using BottleFill.Levels;
using BottleFill.Model;
using BottleFill.Model.Reactions;
using BottleFill.UI;
using UnityEngine;

namespace BottleFill.Gameplay
{
    /// <summary>
    /// Level sahnesindeki tum parcalari olusturup birbirine baglar.
    /// Kural bilmez: damlayi DropResolver'a verir, cikan olaylari dagitir.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        private enum State { Loading, Playing, Won, Lost }

        [SerializeField] private GameConfig config;
        [SerializeField] private LevelHud hud;

        [Header("Siseler")]
        [SerializeField] private BottleView bottlePrefab;
        [SerializeField] private Transform bottleRoot;
        [Tooltip("Level ozel dizilim kullanmiyorsa siseler arasi mesafe.")]
        [SerializeField] private float bottleSpacing = 1.5f;

        [Header("Musluklar")]
        [SerializeField] private DispenserView dispenserPrefab;
        [SerializeField] private Transform dispenserRoot;
        [Tooltip("Level ozel dizilim kullanmiyorsa musluklar arasi mesafe.")]
        [SerializeField] private float dispenserSpacing = 2f;

        private readonly List<BottleView> bottleViews = new List<BottleView>();
        private readonly List<DispenserView> dispenserViews = new List<DispenserView>();
        private ILevelRepository repository;
        private LevelData data;
        private Level level;
        private DropResolver resolver;
        private int levelIndex;
        private State state = State.Loading;

        private async void Start()
        {
            hud.BackClicked += SceneFlow.LoadMain;
            hud.NextLevelClicked += () => SceneFlow.LoadLevel(levelIndex + 1);

            try
            {
                await GameServices.InitializeAsync(config);

                repository = config.CreateLevelRepository();
                levelIndex = SceneFlow.SelectedLevelIndex;

                data = await repository.LoadLevelAsync(levelIndex);
                if (this == null) return; // Yukleme sirasinda sahne degistiyse dur.

                level = new LevelFactory(config.ColorRegistry, config.Scoring).Build(data);
                resolver = DropResolver.CreateDefault(config.MixTable);

                SpawnBottles();
                SpawnDispensers();
                BindHud();
                state = State.Playing;
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        private void SpawnBottles()
        {
            int count = level.Bottles.Count;
            float startX = -(count - 1) * bottleSpacing / 2f;

            for (int i = 0; i < count; i++)
            {
                var view = Instantiate(bottlePrefab, bottleRoot);

                if (data.customLayout)
                {
                    var bottleData = data.bottles[i];
                    float scale = bottleData.scale > 0f ? bottleData.scale : 1f;

                    view.transform.localPosition = new Vector3(bottleData.x, bottleData.y, 0f);
                    view.transform.localScale = Vector3.one * scale;
                }
                else
                {
                    view.transform.localPosition = new Vector3(startX + i * bottleSpacing, 0f, 0f);
                }

                view.Bind(level.Bottles[i]);
                bottleViews.Add(view);
            }
        }

        private void SpawnDispensers()
        {
            int count = level.Dispensers.Count;
            float startX = -(count - 1) * dispenserSpacing / 2f;

            for (int i = 0; i < count; i++)
            {
                var view = Instantiate(dispenserPrefab, dispenserRoot);

                bool hasCustomPosition = data.customLayout && i < data.dispensers.Count;
                view.transform.localPosition = hasCustomPosition
                    ? new Vector3(data.dispensers[i].x, data.dispensers[i].y, 0f)
                    : new Vector3(startX + i * dispenserSpacing, 0f, 0f);

                view.Bind(level.Dispensers[i]);
                view.DropReleased += OnDropReleased;
                dispenserViews.Add(view);
            }
        }

        private void BindHud()
        {
            hud.SetTitle($"Level {levelIndex + 1}");
            hud.SetScore(level.Score.Score);
            hud.SetGoals(level.Goals);

            level.Score.Changed += hud.SetScore;
            level.Goals.Completed += OnLevelCompleted;
        }

        private void OnDropReleased(DispenserView dispenser, BottleView bottle)
        {
            if (state != State.Playing) return;

            var dropped = dispenser.Drops.Current;
            var topBefore = bottle.Model.Top;

            var events = resolver.Resolve(bottle.Model, dropped);
            dispenser.Drops.Advance();

            level.Score.Apply(events);
            level.Goals.OnReactions(events);

            bottle.Refresh();
            dispenser.Refresh();
            hud.SetGoals(level.Goals);

            Debug.Log($"Damla: {dropped.name} | Ustteki: {(topBefore != null ? topBefore.name : "bos")} " +
                      $"| Tepkiler: {string.Join(", ", events)}");

            foreach (var e in events)
                if (e.Type == ReactionType.Overflowed)
                    Debug.LogWarning("Sise tasti. Kaybetme kosulu henuz tanimlanmadi.");
        }

        private void OnLevelCompleted()
        {
            state = State.Won;
            foreach (var dispenser in dispenserViews) dispenser.SetInteractable(false);

            int reward = data.reward > 0 ? data.reward : config.DefaultLevelReward;
            int earned = GameServices.Instance.ReportLevelCompleted(levelIndex, reward);

            hud.ShowWin(levelIndex + 1 < repository.LevelCount, earned);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Play modunda sise ve musluklari Scene penceresinde tasi, siseleri boyutlandir,
        /// sonra bu bilesenin uc nokta menusunden calistir. Yerlesim level asset'ine yazilir.
        /// </summary>
        [ContextMenu("Save Layout To Level")]
        private void SaveLayout()
        {
            if (!Application.isPlaying || bottleViews.Count == 0)
            {
                Debug.LogWarning("Once Play moduna gir ve leveli yukle.");
                return;
            }

            var definition = config.LevelCatalog.Levels[levelIndex];

            var bottlePositions = new List<Vector2>();
            var bottleScales = new List<float>();
            foreach (var view in bottleViews)
            {
                var t = view.transform;
                bottlePositions.Add(t.localPosition);
                bottleScales.Add(t.localScale.x);

                if (!Mathf.Approximately(t.localScale.x, t.localScale.y))
                    Debug.LogWarning($"{view.name} esit olmayan boyutlandirilmis; sadece X degeri kaydedildi.", view);
            }

            var dispenserPositions = new List<Vector2>();
            foreach (var view in dispenserViews)
                dispenserPositions.Add(view.transform.localPosition);

            definition.SetLayout(bottlePositions, bottleScales, dispenserPositions);
            Debug.Log($"{bottlePositions.Count} sise ve {dispenserPositions.Count} musluk " +
                      $"{definition.name} asset'ine kaydedildi.", definition);
        }
#endif
    }
}
