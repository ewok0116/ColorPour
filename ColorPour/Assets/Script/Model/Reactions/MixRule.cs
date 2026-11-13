using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    /// <summary>
    /// Farkli iki ana renk karisip ara renk olur. Yeni ara rengin altinda
    /// ayni ara renk varsa: normalse ikisi patlar, donmussa buzu kirilir.
    /// Donmus katman karismaz.
    /// </summary>
    public class MixRule : IReactionRule
    {
        private readonly ColorMixTable mixTable;

        public MixRule(ColorMixTable mixTable) => this.mixTable = mixTable;

        public bool TryApply(BottleModel bottle, ColorDefinition incoming, List<ReactionEvent> events)
        {
            var top = bottle.Top;
            if (top == null || bottle.IsTopFrozen) return false;
            if (top.IsMixed || incoming.IsMixed) return false;
            if (!mixTable.TryMix(top, incoming, out var mixed)) return false;

            bottle.Pop();
            events.Add(new ReactionEvent(ReactionType.Mixed, mixed));

            if (bottle.Top != mixed)
            {
                bottle.Push(mixed);
            }
            else if (bottle.IsTopFrozen)
            {
                bottle.ThawTop();
                events.Add(new ReactionEvent(ReactionType.IceBroken, mixed));
            }
            else
            {
                bottle.Pop();
                events.Add(new ReactionEvent(ReactionType.Exploded, mixed));
            }
            return true;
        }
    }
}
