using System;
using System.Collections.Generic;

namespace BottleFill.Meta.Lab
{
    public enum PurchaseStatus
    {
        Available,      // Alinabilir
        Owned,          // Zaten alinmis
        NotEnoughCoins, // Para yetmiyor
        LevelLocked,    // Yeterli level tamamlanmamis
        Locked          // On kosul item alinmamis
    }

    /// <summary>
    /// Laboratuvarin durumu: hangi item'lar alinmis, hangi temadayiz,
    /// ne satin alinabilir. Aktif tema kaydedilmez; alinan item'lardan hesaplanir.
    /// </summary>
    public class LabModel
    {
        private readonly LabCatalog catalog;
        private readonly HashSet<string> owned;
        private readonly Wallet wallet;
        private readonly LevelProgress progress;

        public event Action<LabUpgradeDefinition> Purchased;

        /// <summary>Bir tema tamamlanip yenisine gecildiginde. Parametre yeni tema.</summary>
        public event Action<LabTheme> ThemeChanged;

        public LabModel(LabCatalog catalog, IEnumerable<string> ownedIds, Wallet wallet, LevelProgress progress)
        {
            this.catalog = catalog;
            owned = new HashSet<string>(ownedIds);
            this.wallet = wallet;
            this.progress = progress;
        }

        public IEnumerable<string> OwnedIds => owned;

        /// <summary>Tum item'lari alinmamis ilk tema. Hepsi bittiyse son tema.</summary>
        public LabTheme CurrentTheme
        {
            get
            {
                if (catalog == null) return null;

                var themes = catalog.Themes;
                foreach (var theme in themes)
                    if (theme != null && !IsThemeComplete(theme)) return theme;
                return themes.Count > 0 ? themes[themes.Count - 1] : null;
            }
        }

        public bool IsOwned(LabUpgradeDefinition upgrade) => owned.Contains(upgrade.Id);

        public bool IsThemeComplete(LabTheme theme)
        {
            foreach (var upgrade in theme.Upgrades)
                if (upgrade != null && !IsOwned(upgrade)) return false;
            return true;
        }

        public PurchaseStatus GetStatus(LabUpgradeDefinition upgrade)
        {
            if (IsOwned(upgrade)) return PurchaseStatus.Owned;

            // Leveller sirayla acildigi icin HighestUnlocked = tamamlanan level sayisi.
            if (progress.HighestUnlocked < upgrade.UnlockAfterLevel) return PurchaseStatus.LevelLocked;

            if (upgrade.Requires != null && !IsOwned(upgrade.Requires)) return PurchaseStatus.Locked;
            if (!wallet.CanAfford(upgrade.Cost)) return PurchaseStatus.NotEnoughCoins;
            return PurchaseStatus.Available;
        }

        public bool TryPurchase(LabUpgradeDefinition upgrade)
        {
            if (GetStatus(upgrade) != PurchaseStatus.Available) return false;

            var themeBefore = CurrentTheme;
            if (!wallet.TrySpend(upgrade.Cost)) return false;

            owned.Add(upgrade.Id);
            Purchased?.Invoke(upgrade);

            var themeAfter = CurrentTheme;
            if (themeAfter != themeBefore) ThemeChanged?.Invoke(themeAfter);
            return true;
        }
    }
}
