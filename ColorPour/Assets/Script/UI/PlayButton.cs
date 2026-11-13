using System;
using BottleFill.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BottleFill.UI
{
    /// <summary>Main sahnesinde siradaki leveli acan buton: "Level 7".</summary>
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private TMP_Text label;

        private async void Start()
        {
            var button = GetComponent<Button>();
            button.interactable = false;

            try
            {
                var services = await GameServices.InitializeAsync(config);
                if (this == null) return;

                int levelCount = config.CreateLevelRepository().LevelCount;
                int next = services.Progress.HighestUnlocked;

                if (next >= levelCount)
                {
                    label.text = "Yeni leveller yakinda";
                    return;
                }

                label.text = $"Level {next + 1}";
                button.interactable = true;
                button.onClick.AddListener(() => SceneFlow.LoadLevel(next));
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }
    }
}
