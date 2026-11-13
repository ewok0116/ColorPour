using System.Collections.Generic;
using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>
    /// Oyundaki levellerin sirali listesi. Level sirasini degistirmek icin
    /// listede surukle birak yeterli; Resources klasorune gerek yok.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelCatalog", menuName = "Bottle Fill/Level Catalog")]
    public class LevelCatalog : ScriptableObject
    {
        [SerializeField] private List<LevelDefinition> levels = new List<LevelDefinition>();

        public IReadOnlyList<LevelDefinition> Levels => levels;
    }
}
