using System;
using System.Collections.Generic;
using BottleFill.Data;

namespace BottleFill.Model
{
    /// <summary>
    /// Bir muslugun yukaridan gelecek damla sirasi. Ayni seed her zaman ayni sirayi uretir.
    /// </summary>
    public class DropSequence
    {
        private readonly IReadOnlyList<ColorDefinition> pool;
        private readonly Random random;

        public ColorDefinition Current { get; private set; }
        public ColorDefinition Next { get; private set; }

        public DropSequence(IReadOnlyList<ColorDefinition> pool, int seed)
        {
            if (pool == null || pool.Count == 0)
                throw new ArgumentException("Levelde hic damla rengi tanimlanmamis.");

            this.pool = pool;
            random = new Random(seed);
            Current = Pick();
            Next = Pick();
        }

        public void Advance()
        {
            Current = Next;
            Next = Pick();
        }

        private ColorDefinition Pick() => pool[random.Next(pool.Count)];
    }
}
