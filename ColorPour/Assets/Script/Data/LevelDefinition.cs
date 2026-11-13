using System;
using System.Collections.Generic;
using BottleFill.Data.Goals;
using BottleFill.Levels;
using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>
    /// Editorde level tasarlamak icin kullanilan asset. Oyun bunu dogrudan
    /// kullanmaz; ToData() ile duz LevelData'ya cevrilir.
    /// </summary>
    [CreateAssetMenu(fileName = "Level_", menuName = "Bottle Fill/Level Definition")]
    public class LevelDefinition : ScriptableObject
    {
        [Serializable]
        public class LayerSetup
        {
            public ColorDefinition color;
            public bool frozen;
        }

        [Serializable]
        public class BottleSetup
        {
            [Min(1)] public int capacity = 4;

            [Tooltip("Use Custom Layout isaretliyse sisenin konumu.")]
            public Vector2 position;

            [Tooltip("Use Custom Layout isaretliyse sisenin boyutu. 1 = prefab'in kendi boyutu.")]
            [Min(0.01f)] public float scale = 1f;

            [Tooltip("Asagidan yukariya baslangic katmanlari.")]
            public List<LayerSetup> initialLayers = new List<LayerSetup>();
        }

        [Serializable]
        public class DispenserSetup
        {
            [Tooltip("Use Custom Layout isaretliyse muslugun konumu.")]
            public Vector2 position;

            [Tooltip("Bu muslugun verebilecegi renkler. Bos birakilirsa levelin Drop Colors listesi kullanilir.")]
            public List<ColorDefinition> colors = new List<ColorDefinition>();
        }

        [Tooltip("Bos birakilirsa dosya adi kullanilir.")]
        [SerializeField] private string levelId;

        [Tooltip("Ayni seed her seferinde ayni damla sirasini uretir.")]
        [SerializeField] private int seed;

        [Tooltip("Isaretliyse siseler ve musluklar kendi pozisyonlarina, degilse otomatik dizilir.")]
        [SerializeField] private bool useCustomLayout;

        [Tooltip("Ilk tamamlanista verilecek para. 0 ise GameConfig'teki varsayilan kullanilir.")]
        [Min(0)] [SerializeField] private int reward;

        [SerializeField] private List<BottleSetup> bottles = new List<BottleSetup>();

        [Tooltip("Varsayilan renk havuzu. Kendi rengi olmayan musluklar bunu kullanir.")]
        [SerializeField] private List<ColorDefinition> dropColors = new List<ColorDefinition>();

        [Tooltip("Bos birakilirsa tek bir musluk olusturulur.")]
        [SerializeField] private List<DispenserSetup> dispensers = new List<DispenserSetup>();

        [Tooltip("Levelin kazanilmasi icin hepsinin tamamlanmasi gereken hedefler.")]
        [SerializeField] private List<GoalDefinition> goals = new List<GoalDefinition>();

        public LevelData ToData()
        {
            var data = new LevelData
            {
                id = string.IsNullOrWhiteSpace(levelId) ? name : levelId,
                seed = seed,
                customLayout = useCustomLayout,
                reward = reward
            };

            foreach (var bottle in bottles)
            {
                var bottleData = new BottleData
                {
                    capacity = bottle.capacity,
                    x = bottle.position.x,
                    y = bottle.position.y,
                    scale = bottle.scale > 0f ? bottle.scale : 1f
                };

                foreach (var layer in bottle.initialLayers)
                {
                    if (layer.color == null)
                    {
                        Debug.LogWarning($"{name}: bos renkli bir katman atlandi.", this);
                        continue;
                    }
                    bottleData.layers.Add(new LayerData { color = layer.color.Id, frozen = layer.frozen });
                }
                data.bottles.Add(bottleData);
            }

            foreach (var color in dropColors)
                if (color != null) data.dropColors.Add(color.Id);

            foreach (var dispenser in dispensers)
            {
                var dispenserData = new DispenserData { x = dispenser.position.x, y = dispenser.position.y };
                foreach (var color in dispenser.colors)
                    if (color != null) dispenserData.colors.Add(color.Id);
                data.dispensers.Add(dispenserData);
            }

            foreach (var goal in goals)
                if (goal != null) data.goals.Add(goal.ToData());

            return data;
        }

#if UNITY_EDITOR
        /// <summary>Play modunda yerlestirilen sise ve musluklari bu asset'e kalici olarak yazar.</summary>
        public void SetLayout(IReadOnlyList<Vector2> bottlePositions, IReadOnlyList<float> bottleScales,
            IReadOnlyList<Vector2> dispenserPositions)
        {
            UnityEditor.Undo.RecordObject(this, "Save Level Layout");

            for (int i = 0; i < bottles.Count && i < bottlePositions.Count; i++)
            {
                bottles[i].position = bottlePositions[i];
                bottles[i].scale = bottleScales[i];
            }

            // Hic musluk tanimlanmamissa otomatik olusan musluk icin bir kayit ac.
            while (dispensers.Count < dispenserPositions.Count)
                dispensers.Add(new DispenserSetup());

            for (int i = 0; i < dispenserPositions.Count; i++)
                dispensers[i].position = dispenserPositions[i];

            useCustomLayout = true;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }
#endif

        /// <summary>
        /// Inspector'da asset'in uc nokta menusunden calistirilir. Levelin JSON halini
        /// Console'a yazar ve panoya kopyalar; ileride sunucuya yuklemek icin.
        /// </summary>
        [ContextMenu("Copy Level JSON")]
        private void CopyJson()
        {
            var json = JsonUtility.ToJson(ToData(), true);
            GUIUtility.systemCopyBuffer = json;
            Debug.Log($"{name} JSON panoya kopyalandi:\n{json}", this);
        }
    }
}
