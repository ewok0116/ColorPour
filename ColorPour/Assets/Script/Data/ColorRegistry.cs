using System.Collections.Generic;
using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>
    /// Oyundaki tum renklerin listesi. Duz veride (JSON) gelen "red" gibi
    /// id'leri gercek ColorDefinition asset'ine cevirir.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorRegistry", menuName = "Bottle Fill/Color Registry")]
    public class ColorRegistry : ScriptableObject
    {
        [SerializeField] private List<ColorDefinition> colors = new List<ColorDefinition>();

        private Dictionary<string, ColorDefinition> lookup;

        public IReadOnlyList<ColorDefinition> Colors => colors;

        public bool TryGet(string id, out ColorDefinition color)
        {
            if (lookup == null) BuildLookup();
            return lookup.TryGetValue(id ?? "", out color);
        }

        public ColorDefinition Get(string id)
        {
            if (TryGet(id, out var color)) return color;
            throw new KeyNotFoundException(
                $"Bilinmeyen renk id'si: '{id}'. Bu renk ColorRegistry listesine eklendi mi?");
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<string, ColorDefinition>();
            foreach (var color in colors)
            {
                if (color == null) continue;

                if (string.IsNullOrWhiteSpace(color.Id))
                    Debug.LogWarning($"'{color.name}' renginin id'si bos.", color);
                else if (lookup.ContainsKey(color.Id))
                    Debug.LogError($"Ayni id'ye sahip iki renk var: '{color.Id}'", color);
                else
                    lookup.Add(color.Id, color);
            }
        }

        private void OnValidate() => lookup = null;
    }
}
