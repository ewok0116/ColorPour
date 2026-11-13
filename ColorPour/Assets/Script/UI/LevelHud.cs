using System;
using System.Text;
using BottleFill.Model.Goals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BottleFill.UI
{
    /// <summary>
    /// Level sahnesinin arayuzu. Sadece gosterir ve buton olaylarini yayinlar;
    /// oyun mantigina dokunmaz.
    /// </summary>
    public class LevelHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text goalsText;
        [SerializeField] private Button backButton;

        [Header("Kazanma paneli")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private Button nextLevelButton;

        [Tooltip("Istege bagli: kazanilan parayi gosteren yazi.")]
        [SerializeField] private TMP_Text rewardText;

        public event Action BackClicked;
        public event Action NextLevelClicked;

        private void Awake()
        {
            backButton.onClick.AddListener(() => BackClicked?.Invoke());
            nextLevelButton.onClick.AddListener(() => NextLevelClicked?.Invoke());
            winPanel.SetActive(false);
        }

        public void SetTitle(string title) => titleText.text = title;

        public void SetScore(int score) => scoreText.text = $"Puan: {score}";

        public void SetGoals(LevelGoals goals)
        {
            var sb = new StringBuilder();
            foreach (var tracker in goals.Trackers)
                sb.AppendLine($"{(tracker.IsCompleted ? "✓" : "•")} {tracker.ProgressText}");
            goalsText.text = sb.ToString();
        }

        public void ShowWin(bool hasNextLevel, int earnedCoins)
        {
            winPanel.SetActive(true);
            nextLevelButton.gameObject.SetActive(hasNextLevel);

            if (rewardText != null)
                rewardText.text = earnedCoins > 0 ? $"+{earnedCoins} para" : "Bu levelin odulu daha once alindi";
        }
    }
}
