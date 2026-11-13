using System;
using BottleFill.Core;
using UnityEngine;

namespace BottleFill.UI
{
    /// <summary>Her level icin bir buton olusturur; acilmamislari kilitler.</summary>
    public class LevelSelectScreen : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private LevelButton buttonPrefab;
        [SerializeField] private Transform buttonContainer;

        private async void Start()
        {
            try
            {
                var services = await GameServices.InitializeAsync(config);
                if (this == null) return;

                var repository = config.CreateLevelRepository();
                for (int i = 0; i < repository.LevelCount; i++)
                {
                    var button = Instantiate(buttonPrefab, buttonContainer);
                    button.Setup(i, services.Progress.IsUnlocked(i), SceneFlow.LoadLevel);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }
    }
}
