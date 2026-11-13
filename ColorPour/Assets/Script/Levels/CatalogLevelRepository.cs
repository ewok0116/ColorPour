using System;
using System.Threading.Tasks;
using BottleFill.Data;

namespace BottleFill.Levels
{
    /// <summary>Levelleri editorde hazirlanan LevelCatalog asset'inden okur. Simdilik varsayilan kaynak.</summary>
    public class CatalogLevelRepository : ILevelRepository
    {
        private readonly LevelCatalog catalog;

        public CatalogLevelRepository(LevelCatalog catalog) => this.catalog = catalog;

        public int LevelCount => catalog.Levels.Count;

        public Task<LevelData> LoadLevelAsync(int index)
        {
            if (index < 0 || index >= LevelCount)
                throw new ArgumentOutOfRangeException(nameof(index), $"Level {index} yok, toplam {LevelCount} level var.");

            return Task.FromResult(catalog.Levels[index].ToData());
        }
    }
}
