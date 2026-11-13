using System;
using BottleFill.Core;
using BottleFill.Meta;
using TMPro;
using UnityEngine;

namespace BottleFill.UI
{
    /// <summary>Oyuncunun parasini gosterir ve degistikce gunceller.</summary>
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private TMP_Text text;

        private Wallet wallet;

        private async void Start()
        {
            try
            {
                var services = await GameServices.InitializeAsync(config);
                if (this == null) return;

                wallet = services.Wallet;
                wallet.Changed += Show;
                Show(wallet.Coins);
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        // Cuzdan sahneden uzun yasar; sahne kapaninca aboneligi birakmazsak hata aliriz.
        private void OnDestroy()
        {
            if (wallet != null) wallet.Changed -= Show;
        }

        private void Show(int coins) => text.text = coins.ToString();
    }
}
