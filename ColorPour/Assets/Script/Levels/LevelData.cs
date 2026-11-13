using System;
using System.Collections.Generic;

namespace BottleFill.Levels
{
    // Bu siniflar sadece duz veridir: Unity asset referansi icermez,
    // bu yuzden JSON'a cevrilebilir ve sunucudan gelebilir.
    // Alan adlari JSON anahtarlari oldugu icin sonradan degistirme.

    [Serializable]
    public class LevelData
    {
        public string id;
        public int seed;
        public bool customLayout;

        /// <summary>Ilk tamamlanista verilecek para. 0 ise GameConfig'teki varsayilan kullanilir.</summary>
        public int reward;

        public List<BottleData> bottles = new List<BottleData>();

        /// <summary>Varsayilan renk havuzu. Kendi rengi olmayan musluklar bunu kullanir.</summary>
        public List<string> dropColors = new List<string>();

        /// <summary>Bossa tek bir musluk olusturulur.</summary>
        public List<DispenserData> dispensers = new List<DispenserData>();

        public List<GoalData> goals = new List<GoalData>();
    }

    [Serializable]
    public class BottleData
    {
        public int capacity = 4;
        public float x;
        public float y;
        public float scale = 1f;
        public List<LayerData> layers = new List<LayerData>();
    }

    [Serializable]
    public class LayerData
    {
        public string color;
        public bool frozen;
    }

    [Serializable]
    public class DispenserData
    {
        public float x;
        public float y;

        /// <summary>Bossa levelin dropColors listesi kullanilir.</summary>
        public List<string> colors = new List<string>();
    }

    [Serializable]
    public class GoalData
    {
        public string type;
        public int target;

        public GoalData() { }

        public GoalData(string type, int target = 0)
        {
            this.type = type;
            this.target = target;
        }
    }

    /// <summary>GoalData.type icin gecerli degerler. Yazim hatasini onlemek icin hep bunlari kullan.</summary>
    public static class GoalTypes
    {
        public const string Score = "score";
        public const string BreakIce = "break_ice";
    }
}
