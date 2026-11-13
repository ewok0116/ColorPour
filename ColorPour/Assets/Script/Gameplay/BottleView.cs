using System;
using System.Collections.Generic;
using BottleFill.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BottleFill.Gameplay
{
    /// <summary>
    /// Bir siseyi cizer. Kural bilmez, karar vermez;
    /// sadece BottleModel'deki katmanlari LiquidBottom ile LiquidTop arasina dizer.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class BottleView : MonoBehaviour, IPointerClickHandler
    {
        [Tooltip("Sisenin cam gorseli. Sivi her zaman bunun arkasinda cizilir.")]
        [SerializeField] private SpriteRenderer glass;

        [Header("Sivi")]
        [Tooltip("Duz renkli kare sprite'li prefab. Sise gorseli DEGIL.")]
        [SerializeField] private SpriteRenderer layerPrefab;

        [Tooltip("Sivinin basladigi nokta: sisenin ic tabani.")]
        [SerializeField] private Transform liquidBottom;

        [Tooltip("Sise tamamen doluyken sivinin ulastigi nokta.")]
        [SerializeField] private Transform liquidTop;

        [SerializeField] private float liquidWidth = 0.8f;

        [Tooltip("Katmanlar arasi bosluk. Sivi icin 0 birak.")]
        [SerializeField] private float layerSpacing = 0f;

        [Header("Buz")]
        [SerializeField] private Color frozenTint = new Color(0.8f, 0.95f, 1f);
        [Range(0f, 1f)] [SerializeField] private float frozenTintAmount = 0.6f;

        private readonly List<SpriteRenderer> spawnedLayers = new List<SpriteRenderer>();

        public BottleModel Model { get; private set; }
        public event Action<BottleView> Clicked;

        private float LiquidHeight => liquidTop.localPosition.y - liquidBottom.localPosition.y;

        public void Bind(BottleModel model)
        {
            Model = model;
            Refresh();
        }

        /// <summary>Gorseli modelin su anki haline gore yeniden cizer.</summary>
        public void Refresh()
        {
            // Simdilik her seferinde bastan ciziyoruz; animasyon adiminda Object Pool'a gececegiz.
            foreach (var layer in spawnedLayers) Destroy(layer.gameObject);
            spawnedLayers.Clear();

            float layerHeight = LiquidHeight / Model.Capacity;
            Vector3 bottom = liquidBottom.localPosition;

            for (int i = 0; i < Model.Layers.Count; i++)
            {
                var layer = Model.Layers[i];
                var renderer = Instantiate(layerPrefab, transform);

                float y = bottom.y + i * layerHeight + layerHeight / 2f;
                renderer.transform.localPosition = new Vector3(bottom.x, y, 0f);
                renderer.transform.localScale = new Vector3(liquidWidth, layerHeight - layerSpacing, 1f);

                var color = layer.Color.DisplayColor;
                if (layer.IsFrozen) color = Color.Lerp(color, frozenTint, frozenTintAmount);
                renderer.color = color;

                // Sivi her zaman camin bir arkasinda cizilsin.
                if (glass != null)
                {
                    renderer.sortingLayerID = glass.sortingLayerID;
                    renderer.sortingOrder = glass.sortingOrder - 1;
                }

                spawnedLayers.Add(renderer);
            }
        }

        public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(this);

        /// <summary>Prefab'i duzenlerken sivi alanini mavi dikdortgen olarak gosterir.</summary>
        private void OnDrawGizmosSelected()
        {
            if (liquidBottom == null || liquidTop == null) return;

            Vector3 b = liquidBottom.position;
            Vector3 t = liquidTop.position;
            var center = new Vector3(b.x, (b.y + t.y) / 2f, b.z);
            var size = new Vector3(liquidWidth * transform.lossyScale.x, t.y - b.y, 0f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
