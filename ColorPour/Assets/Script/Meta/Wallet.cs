using System;

namespace BottleFill.Meta
{
    /// <summary>
    /// Oyuncunun parasi. Para sadece buradan eklenir ve harcanir;
    /// UI parayi dogrudan degistirmez, Changed olayini dinler.
    /// </summary>
    public class Wallet
    {
        public int Coins { get; private set; }
        public event Action<int> Changed;

        public Wallet(int coins) => Coins = Math.Max(0, coins);

        public bool CanAfford(int amount) => amount <= Coins;

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Coins += amount;
            Changed?.Invoke(Coins);
        }

        public bool TrySpend(int amount)
        {
            if (amount < 0 || !CanAfford(amount)) return false;
            Coins -= amount;
            Changed?.Invoke(Coins);
            return true;
        }
    }
}
