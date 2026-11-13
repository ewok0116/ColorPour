using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace BottleFill.Core.Save
{
    /// <summary>Oyuncu verisini cihazda bir JSON dosyasina kaydeder.</summary>
    public class FileSaveStorage : ISaveStorage
    {
        public const string DefaultFileName = "save.json";

        private readonly string path;

        public FileSaveStorage(string fileName = DefaultFileName)
        {
            path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public Task<PlayerData> LoadAsync()
        {
            try
            {
                if (!File.Exists(path)) return Task.FromResult(new PlayerData());

                var data = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
                return Task.FromResult(data ?? new PlayerData());
            }
            catch (Exception e)
            {
                Debug.LogError($"Kayit okunamadi, yeni kayitla baslaniyor: {e.Message}");
                return Task.FromResult(new PlayerData());
            }
        }

        public Task SaveAsync(PlayerData data)
        {
            // Once gecici dosyaya yaz, sonra yer degistir: yazma yarida kesilirse eski kayit bozulmaz.
            var temp = path + ".tmp";
            File.WriteAllText(temp, JsonUtility.ToJson(data, true));
            if (File.Exists(path)) File.Delete(path);
            File.Move(temp, path);
            return Task.CompletedTask;
        }

        public static void DeleteDefaultFile()
        {
            var file = Path.Combine(Application.persistentDataPath, DefaultFileName);
            if (File.Exists(file)) File.Delete(file);
            Debug.Log($"Kayit silindi: {file}");
        }
    }
}
