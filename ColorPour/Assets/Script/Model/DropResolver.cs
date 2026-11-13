using System.Collections.Generic;
using BottleFill.Data;
using BottleFill.Model.Reactions;

namespace BottleFill.Model
{
    /// <summary>
    /// Chain of Responsibility: kurallari sirayla dener, uyan ilk kural calisir.
    /// Yeni bir mekanik eklemek = yeni bir IReactionRule yazip listeye eklemek.
    /// </summary>
    public class DropResolver
    {
        private readonly IReadOnlyList<IReactionRule> rules;

        public DropResolver(IReadOnlyList<IReactionRule> rules) => this.rules = rules;

        /// <summary>Oyunun standart kural sirasi. Sira onemlidir.</summary>
        public static DropResolver CreateDefault(ColorMixTable mixTable) =>
            new DropResolver(new IReactionRule[]
            {
                new IceBreakRule(),     // Donmus + ayni renk
                new EvaporateRule(),    // Ana renk + ayni ana renk
                new MixRule(mixTable),  // Ana renk + farkli ana renk
                new StackRule()         // Diger her durum (donmus + farkli renk dahil)
            });

        public IReadOnlyList<ReactionEvent> Resolve(BottleModel bottle, ColorDefinition incoming)
        {
            var events = new List<ReactionEvent>();
            foreach (var rule in rules)
            {
                if (rule.TryApply(bottle, incoming, events)) break;
            }
            return events;
        }
    }
}
