using System;
using System.Collections.Generic;
using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>
    /// Hangi iki ana rengin hangi ara rengi olusturdugunu tutar.
    /// Siralama onemsizdir: Kirmizi + Mavi ile Mavi + Kirmizi ayni sonucu verir.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorMixTable", menuName = "Bottle Fill/Color Mix Table")]
    public class ColorMixTable : ScriptableObject
    {
        [Serializable]
        private class Recipe
        {
            public ColorDefinition first;
            public ColorDefinition second;
            public ColorDefinition result;
        }

        [SerializeField] private List<Recipe> recipes = new List<Recipe>();

        private Dictionary<(ColorDefinition, ColorDefinition), ColorDefinition> lookup;

        public bool TryMix(ColorDefinition a, ColorDefinition b, out ColorDefinition result)
        {
            if (lookup == null) BuildLookup();
            return lookup.TryGetValue((a, b), out result);
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<(ColorDefinition, ColorDefinition), ColorDefinition>();
            foreach (var recipe in recipes)
            {
                if (recipe.first == null || recipe.second == null || recipe.result == null) continue;
                lookup[(recipe.first, recipe.second)] = recipe.result;
                lookup[(recipe.second, recipe.first)] = recipe.result;
            }
        }

        // Inspector'da tablo degisirse onbellek yenilensin.
        private void OnValidate() => lookup = null;
    }
}
