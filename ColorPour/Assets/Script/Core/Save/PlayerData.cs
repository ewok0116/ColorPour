using System;
using System.Collections.Generic;

namespace BottleFill.Core.Save
{
    /// <summary>
    /// Oyuncunun kalici verisi. Duz veri oldugu icin dosyaya ya da sunucuya
    /// JSON olarak yazilabilir. Alan adlarini sonradan degistirme.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public int version = 1;
        public int coins;

        /// <summary>Acik olan en yuksek level (0 = sadece ilk level acik).</summary>
        public int highestUnlockedLevel;

        public List<string> ownedLabUpgrades = new List<string>();
    }
}
