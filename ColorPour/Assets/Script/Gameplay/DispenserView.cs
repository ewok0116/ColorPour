using System;
using BottleFill.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BottleFill.Gameplay
{
    /// <summary>
    /// Bir muslugu gosterir ve damlasinin suruklenmesini yonetir.
    /// Kural bilmez: damla bir sisenin ustune birakilinca sadece haber verir.
    /// </summary>
    public class DispenserView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Tooltip("Suruklenen damla. Uzerinde Collider2D olmali; gorseli beyaz olmali.")]
        [SerializeField] private SpriteRenderer drop;

        [Tooltip("Istege bagli: siradaki damlayi gosteren kucuk sprite.")]
        [SerializeField] private SpriteRenderer nextDrop;

        [Tooltip("Surukleme sirasinda damla parmagin ne kadar ustunde dursun (dunya birimi).")]
        [SerializeField] private Vector2 dragOffset = new Vector2(0f, 0.4f);

        [Tooltip("Suruklenen damlanin sise ve camlarin onunde gorunmesi icin sira.")]
        [SerializeField] private int draggingSortingOrder = 100;

        private Camera cam;
        private Vector3 dropHomePosition;
        private int dropDefaultSortingOrder;
        private bool dragging;
        private bool interactable = true;

        public DropSequence Drops { get; private set; }

        /// <summary>Damla bir sisenin ustune birakildi.</summary>
        public event Action<DispenserView, BottleView> DropReleased;

        private void Awake()
        {
            cam = Camera.main;
            dropHomePosition = drop.transform.localPosition;
            dropDefaultSortingOrder = drop.sortingOrder;
        }

        public void Bind(DropSequence drops)
        {
            Drops = drops;
            Refresh();
        }

        public void Refresh()
        {
            drop.color = Drops.Current.DisplayColor;
            if (nextDrop != null) nextDrop.color = Drops.Next.DisplayColor;
        }

        public void SetInteractable(bool value)
        {
            interactable = value;
            if (!value && dragging) ResetDrop();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!interactable) return;

            dragging = true;
            drop.sortingOrder = draggingSortingOrder;
            MoveDropToPointer(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragging) MoveDropToPointer(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging) return;

            // Oyuncunun gordugu yer damlanin kendisi, o yuzden parmagi degil damlayi kontrol ediyoruz.
            var bottle = FindBottleAt(drop.transform.position);
            ResetDrop();

            if (bottle != null) DropReleased?.Invoke(this, bottle);
        }

        private void MoveDropToPointer(PointerEventData eventData)
        {
            var screen = new Vector3(eventData.position.x, eventData.position.y, -cam.transform.position.z);
            var world = cam.ScreenToWorldPoint(screen);
            world.z = drop.transform.position.z;
            drop.transform.position = world + (Vector3)dragOffset;
        }

        private void ResetDrop()
        {
            dragging = false;
            drop.transform.localPosition = dropHomePosition;
            drop.sortingOrder = dropDefaultSortingOrder;
        }

        private static BottleView FindBottleAt(Vector2 point)
        {
            foreach (var hit in Physics2D.OverlapPointAll(point))
            {
                var bottle = hit.GetComponentInParent<BottleView>();
                if (bottle != null) return bottle;
            }
            return null;
        }
    }
}
