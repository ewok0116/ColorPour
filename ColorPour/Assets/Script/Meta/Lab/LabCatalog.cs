using System.Collections.Generic;
using UnityEngine;

namespace BottleFill.Meta.Lab
{
    /// <summary>Oyundaki laboratuvarlar, oyuncunun gececegi sirayla.</summary>
    [CreateAssetMenu(fileName = "LabCatalog", menuName = "Bottle Fill/Lab/Lab Catalog")]
    public class LabCatalog : ScriptableObject
    {
        [SerializeField] private List<LabTheme> themes = new List<LabTheme>();

        public IReadOnlyList<LabTheme> Themes => themes;
    }
}
