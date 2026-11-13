using System.Collections.Generic;
using UnityEngine;

namespace BottleFill.Meta.Lab
{
    /// <summary>
    /// Bir laboratuvar: gorunumu (oda prefab'i) ve item'lari.
    /// Tum item'lari alininca oyuncu bir sonraki temaya gecer.
    /// </summary>
    [CreateAssetMenu(fileName = "Theme_", menuName = "Bottle Fill/Lab/Lab Theme")]
    public class LabTheme : ScriptableObject
    {
        [SerializeField] private string displayName;

        [Tooltip("Bu laboratuvarin gorunumu: oda, raflar, masa ve icindeki LabItemView'lar.")]
        [SerializeField] private GameObject roomPrefab;

        [Tooltip("Bu laboratuvarin item'lari. Satin alma panelinde bu sirayla gorunur.")]
        [SerializeField] private List<LabUpgradeDefinition> upgrades = new List<LabUpgradeDefinition>();

        public string DisplayName => displayName;
        public GameObject RoomPrefab => roomPrefab;
        public IReadOnlyList<LabUpgradeDefinition> Upgrades => upgrades;
    }
}
