using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model
{
    /// <summary>
    /// Bir siseyi temsil eden saf veri modeli. MonoBehaviour degildir,
    /// gorsel ya da animasyon bilgisi icermez. Katmanlar asagidan yukariya tutulur.
    /// </summary>
    public class BottleModel
    {
        private readonly List<Layer> layers = new List<Layer>();

        public int Capacity { get; }
        public IReadOnlyList<Layer> Layers => layers;
        public int Count => layers.Count;
        public bool IsEmpty => layers.Count == 0;
        public bool IsFull => layers.Count >= Capacity;

        /// <summary>En ustteki katman, sise bossa null.</summary>
        public Layer? TopLayer => IsEmpty ? (Layer?)null : layers[layers.Count - 1];

        /// <summary>En ustteki katmanin rengi, sise bossa null.</summary>
        public ColorDefinition Top => TopLayer?.Color;

        public bool IsTopFrozen => TopLayer?.IsFrozen ?? false;

        public BottleModel(int capacity, IEnumerable<Layer> initialLayers = null)
        {
            Capacity = capacity;
            if (initialLayers != null) layers.AddRange(initialLayers);
        }

        public void Push(ColorDefinition color) => layers.Add(new Layer(color));

        public ColorDefinition Pop()
        {
            var top = Top;
            if (!IsEmpty) layers.RemoveAt(layers.Count - 1);
            return top;
        }

        /// <summary>En ustteki katmanin buzunu cozer. Katman yerinde kalir.</summary>
        public void ThawTop()
        {
            if (IsEmpty) return;
            int i = layers.Count - 1;
            layers[i] = layers[i].Thawed();
        }

        public int CountFrozen()
        {
            int count = 0;
            foreach (var layer in layers)
                if (layer.IsFrozen) count++;
            return count;
        }

        /// <summary>Undo icin anlik goruntu.</summary>
        public List<Layer> Snapshot() => new List<Layer>(layers);

        public void Restore(IEnumerable<Layer> snapshot)
        {
            layers.Clear();
            layers.AddRange(snapshot);
        }
    }
}
