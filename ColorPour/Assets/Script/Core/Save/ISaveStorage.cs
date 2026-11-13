using System.Threading.Tasks;

namespace BottleFill.Core.Save
{
    /// <summary>
    /// Oyuncu verisinin nereye kaydedildigini gizler. Simdilik dosya,
    /// ileride sunucu. Oyunun geri kalani farki bilmez.
    /// </summary>
    public interface ISaveStorage
    {
        Task<PlayerData> LoadAsync();
        Task SaveAsync(PlayerData data);
    }
}
