using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BottleFill.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        [Tooltip("Istege bagli: level kilitliyken gosterilecek obje, ornegin kilit ikonu.")]
        [SerializeField] private GameObject lockedOverlay;

        public void Setup(int levelIndex, bool unlocked, Action<int> onClick)
        {
            label.text = (levelIndex + 1).ToString();

            var button = GetComponent<Button>();
            button.interactable = unlocked;
            button.onClick.RemoveAllListeners();
            if (unlocked) button.onClick.AddListener(() => onClick(levelIndex));

            if (lockedOverlay != null) lockedOverlay.SetActive(!unlocked);
        }
    }
}
