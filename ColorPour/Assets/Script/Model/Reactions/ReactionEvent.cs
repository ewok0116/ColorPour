using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    public enum ReactionType
    {
        Stacked,     // Damla ust uste bindi
        Evaporated,  // Ayni ana renkler bulustu, ikisi de yok oldu
        Mixed,       // Iki ana renk karisip ara renk oldu
        Exploded,    // Ayni ara renkler ust uste geldi, ikisi de patladi
        IceBroken,   // Donmus katmanin ustune ayni renk geldi, buz kirildi
        Overflowed   // Sise doluydu, damla tasti
    }

    /// <summary>
    /// Bir kuralin "ne oldugunu" anlatan kayit. Mantik bu listeyi uretir,
    /// gorsel taraf bu listeye bakarak animasyonlari sirayla oynatir.
    /// </summary>
    public readonly struct ReactionEvent
    {
        public ReactionType Type { get; }
        public ColorDefinition Color { get; }

        public ReactionEvent(ReactionType type, ColorDefinition color)
        {
            Type = type;
            Color = color;
        }

        public override string ToString() => $"{Type}({(Color != null ? Color.name : "null")})";
    }
}
