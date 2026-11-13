using System;
using BottleFill.Meta.Lab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BottleFill.UI.Lab
{
    /// <summary>Satin alma panelinde tek bir item'in butonu.</summary>
    [RequireComponent(typeof(Button))]
    public class LabUpgradeButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text priceText;

        private Button button;

        public LabUpgradeDefinition Upgrade { get; private set; }

        public void Setup(LabUpgradeDefinition upgrade, Action<LabUpgradeDefinition> onClick)
        {
            Upgrade = upgrade;
            button = GetComponent<Button>();

            nameText.text = upgrade.DisplayName;
            if (icon != null)
            {
                icon.sprite = upgrade.Icon;
                icon.enabled = upgrade.Icon != null;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick(upgrade));
        }

        public void Refresh(PurchaseStatus status)
        {
            button.interactable = status == PurchaseStatus.Available;

            priceText.text = status switch
            {
                PurchaseStatus.Owned => "Alindi",
                PurchaseStatus.LevelLocked => $"Level {Upgrade.UnlockAfterLevel}",
                PurchaseStatus.Locked => "Kilitli",
                _ => Upgrade.Cost.ToString()
            };
        }
    }
}
