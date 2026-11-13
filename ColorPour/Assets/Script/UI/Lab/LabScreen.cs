using System;
using System.Collections.Generic;
using BottleFill.Core;
using BottleFill.Meta;
using BottleFill.Meta.Lab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BottleFill.UI.Lab
{
    /// <summary>
    /// Main sahnesindeki laboratuvar: aktif temanin odasini yukler, item'lari
    /// satin alinma durumuna gore gosterir, satin alma panelini kurar.
    /// Kural bilmez; LabModel'e sorar.
    /// </summary>
    public class LabScreen : MonoBehaviour
    {
        [SerializeField] private GameConfig config;

        [Tooltip("Aktif temanin oda prefab'inin olusturulacagi yer.")]
        [SerializeField] private Transform roomRoot;

        [Tooltip("Istege bagli: laboratuvarin adini gosteren yazi.")]
        [SerializeField] private TMP_Text themeTitle;

        [Header("Satin alma paneli")]
        [SerializeField] private LabUpgradeButton buttonPrefab;
        [SerializeField] private Transform buttonContainer;

        [Header("Tema tamamlandi (istege bagli)")]
        [Tooltip("Bossa yeni laboratuvara hemen gecilir.")]
        [SerializeField] private GameObject themeCompletedPanel;
        [SerializeField] private Button nextThemeButton;

        private readonly List<LabUpgradeButton> buttons = new List<LabUpgradeButton>();
        private LabItemView[] items = Array.Empty<LabItemView>();
        private GameObject room;
        private LabTheme pendingTheme;
        private LabModel lab;
        private Wallet wallet;

        private async void Start()
        {
            if (themeCompletedPanel != null) themeCompletedPanel.SetActive(false);
            if (nextThemeButton != null) nextThemeButton.onClick.AddListener(OnNextThemeClicked);

            try
            {
                var services = await GameServices.InitializeAsync(config);
                if (this == null) return;

                lab = services.Lab;
                wallet = services.Wallet;

                lab.Purchased += OnPurchased;
                lab.ThemeChanged += OnThemeChanged;
                wallet.Changed += OnCoinsChanged;

                ShowTheme(lab.CurrentTheme);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        // Servisler sahneden uzun yasar; sahne kapaninca aboneligi birakmazsak hata aliriz.
        private void OnDestroy()
        {
            if (lab != null)
            {
                lab.Purchased -= OnPurchased;
                lab.ThemeChanged -= OnThemeChanged;
            }
            if (wallet != null) wallet.Changed -= OnCoinsChanged;
        }

        private void ShowTheme(LabTheme theme)
        {
            if (room != null) Destroy(room);
            foreach (var button in buttons) Destroy(button.gameObject);
            buttons.Clear();
            items = Array.Empty<LabItemView>();

            if (theme == null) return;

            if (themeTitle != null) themeTitle.text = theme.DisplayName;

            if (theme.RoomPrefab != null)
            {
                room = Instantiate(theme.RoomPrefab, roomRoot);
                items = room.GetComponentsInChildren<LabItemView>(true);
            }

            foreach (var upgrade in theme.Upgrades)
            {
                if (upgrade == null) continue;
                var button = Instantiate(buttonPrefab, buttonContainer);
                button.Setup(upgrade, OnBuyClicked);
                buttons.Add(button);
            }

            RefreshAll();
        }

        // Ekrani burada guncellemiyoruz; satin alma basariliysa Purchased olayi gelir.
        private void OnBuyClicked(LabUpgradeDefinition upgrade) => lab.TryPurchase(upgrade);

        private void OnPurchased(LabUpgradeDefinition upgrade) => RefreshAll();

        private void OnCoinsChanged(int coins) => RefreshButtons();

        private void OnThemeChanged(LabTheme newTheme)
        {
            // Panel yoksa hemen gec; varsa oyuncu tamamlanmis laboratuvari gorsun, butona basinca gecsin.
            if (themeCompletedPanel == null)
            {
                ShowTheme(newTheme);
                return;
            }

            pendingTheme = newTheme;
            themeCompletedPanel.SetActive(true);
        }

        private void OnNextThemeClicked()
        {
            themeCompletedPanel.SetActive(false);
            if (pendingTheme != null) ShowTheme(pendingTheme);
            pendingTheme = null;
        }

        private void RefreshAll()
        {
            foreach (var item in items) item.Refresh(lab);
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            foreach (var button in buttons) button.Refresh(lab.GetStatus(button.Upgrade));
        }
    }
}
