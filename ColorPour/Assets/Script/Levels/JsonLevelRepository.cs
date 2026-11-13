using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BottleFill.Levels
{
    /// <summary>
    /// Levelleri JSON metinlerinden okur. Sunucuya gecmeden once JSON formatini
    /// test etmek icin kullanilir; ileride RemoteLevelRepository ayni JSON'u indirip
    /// ayni sekilde cozecek.
    /// </summary>
    public class JsonLevelRepository : ILevelRepository
    {
        private readonly IReadOnlyList<TextAsset> files;

        public JsonLevelRepository(IReadOnlyList<TextAsset> files) => this.files = files;

        public int LevelCount => files.Count;

        public Task<LevelData> LoadLevelAsync(int index)
        {
            if (index < 0 || index >= LevelCount)
                throw new ArgumentOutOfRangeException(nameof(index), $"Level {index} yok, toplam {LevelCount} level var.");

            return Task.FromResult(JsonUtility.FromJson<LevelData>(files[index].text));
        }
    }
}
