using System.Threading.Tasks;

namespace BottleFill.Levels
{
    /// <summary>
    /// Level verisinin nereden geldigini gizler (Repository pattern).
    /// Oyun sadece bu arayuzu bilir; asset, JSON veya sunucu fark etmez.
    /// Async, cunku ileride sunucudan cekmek zaman alacak.
    /// </summary>
    public interface ILevelRepository
    {
        int LevelCount { get; }
        Task<LevelData> LoadLevelAsync(int index);
    }
}
