using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    /// <summary>Ayni ana renk ustteki (donmamis) ana rengin uzerine duserse ikisi buharlasir.</summary>
    public class EvaporateRule : IReactionRule
    {
        public bool TryApply(BottleModel bottle, ColorDefinition incoming, List<ReactionEvent> events)
        {
            var top = bottle.Top;
            if (top == null || bottle.IsTopFrozen) return false;
            if (top.IsMixed || incoming.IsMixed || top != incoming) return false;

            bottle.Pop();
            events.Add(new ReactionEvent(ReactionType.Evaporated, incoming));
            return true;
        }
    }
}
