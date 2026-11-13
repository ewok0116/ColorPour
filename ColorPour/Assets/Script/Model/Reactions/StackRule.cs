using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model.Reactions
{
    /// <summary>
    /// Son kural: baska hicbir kural uymadiysa damla ust uste biner.
    /// Sise doluysa tasar. Her zaman true doner, zincirin sonunda durmali.
    /// </summary>
    public class StackRule : IReactionRule
    {
        public bool TryApply(BottleModel bottle, ColorDefinition incoming, List<ReactionEvent> events)
        {
            if (bottle.IsFull)
            {
                events.Add(new ReactionEvent(ReactionType.Overflowed, incoming));
                return true;
            }

            bottle.Push(incoming);
            events.Add(new ReactionEvent(ReactionType.Stacked, incoming));
            return true;
        }
    }
}
