using UnityEngine;

namespace BottleFill.Meta.Lab
{
    /// <summary>Laboratuvara eklenebilecek tek bir item.</summary>
    [CreateAssetMenu(fileName = "Upgrade_", menuName = "Bottle Fill/Lab/Upgrade")]
    public class LabUpgradeDefinition : ScriptableObject
    {
        [Tooltip("Kalici kimlik, kayitta kullanilir. Tum temalar arasinda benzersiz olmali.")]
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [Min(0)] [SerializeField] private int cost = 100;
        [SerializeField] private Sprite icon;

        [Tooltip("Bu kadar level tamamlaninca satin alinabilir olur. 0 = bastan acik.")]
        [Min(0)] [SerializeField] private int unlockAfterLevel;

        [Tooltip("Istege bagli: once bu item'in alinmis olmasi gerekir.")]
        [SerializeField] private LabUpgradeDefinition requires;

        public string Id => id;
        public string DisplayName => displayName;
        public int Cost => cost;
        public Sprite Icon => icon;
        public int UnlockAfterLevel => unlockAfterLevel;
        public LabUpgradeDefinition Requires => requires;

        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(id)) return;

            // "Upgrade_Microscope" -> "microscope"
            id = name.Replace("Upgrade_", "").Trim().ToLowerInvariant();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
