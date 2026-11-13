using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    /// <summary>
    /// Bir damlanin siseye dusmesiyle ilgili tek bir kural.
    /// Kural uygulanabiliyorsa siseyi degistirir, olaylari listeye ekler ve true doner.
    /// </summary>
    public interface IReactionRule
    {
        bool TryApply(BottleModel bottle, ColorDefinition incoming, List<ReactionEvent> events);
    }
}
