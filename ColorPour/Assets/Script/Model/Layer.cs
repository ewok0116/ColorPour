using BottleFill.Data;

namespace BottleFill.Model
{
    /// <summary>
    /// Sisedeki tek bir katman: rengi ve donmus olup olmadigi.
    /// Degismezdir (immutable); buz cozuldugunde yeni bir Layer olusturulur.
    /// </summary>
    public readonly struct Layer
    {
        public ColorDefinition Color { get; }
        public bool IsFrozen { get; }

        public Layer(ColorDefinition color, bool isFrozen = false)
        {
            Color = color;
            IsFrozen = isFrozen;
        }

        public Layer Thawed() => new Layer(Color, false);

        public override string ToString() =>
            $"{(Color != null ? Color.name : "null")}{(IsFrozen ? " [buz]" : "")}";
    }
}
