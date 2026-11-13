using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    /// <summary>
    /// Donmus katmanin ustune ayni renk gelirse buz kirilir. Katman normal
    /// renge doner, gelen damla harcanir. Zincirde ilk sirada olmali.
    /// </summary>
    public class IceBreakRule : IReactionRule
    {
        public bool TryApply(BottleModel bottle, ColorDefinition incoming, List<ReactionEvent> events)
        {
            if (!bottle.IsTopFrozen || bottle.Top != incoming) return false;

            bottle.ThawTop();
            events.Add(new ReactionEvent(ReactionType.IceBroken, incoming));
            return true;
        }
    }
}
