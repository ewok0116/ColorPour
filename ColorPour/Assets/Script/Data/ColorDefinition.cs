using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>
    /// Oyundaki tek bir rengi tanimlar. Ana renkler (kirmizi, mavi...) ve
    /// karisimdan olusan ara renkler (mor, yesil...) ayni sinifi kullanir.
    /// Id sabittir ve JSON/sunucu verisinde rengi temsil eder; sonradan degistirme.
    /// </summary>
    [CreateAssetMenu(fileName = "Color_", menuName = "Bottle Fill/Color Definition")]
    public class ColorDefinition : ScriptableObject
    {
        [Tooltip("Kalici kimlik, ornegin \"red\" veya \"purple\". Bos birakilirsa dosya adindan uretilir.")]
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private Color displayColor = Color.white;

        [Tooltip("Ara renkler baska bir renkle tekrar karisamaz.")]
        [SerializeField] private bool isMixed;

        public string Id => id;
        public string DisplayName => displayName;
        public Color DisplayColor => displayColor;
        public bool IsMixed => isMixed;

        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(id)) return;

            // "Color_Red" -> "red"
            id = name.Replace("Color_", "").Trim().ToLowerInvariant();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
