using System;
using System.Collections.Generic;
using BottleFill.Model.Reactions;

namespace BottleFill.Model.Goals
{
    /// <summary>Oyun sirasinda tek bir hedefin ilerlemesini takip eder.</summary>
    public interface IGoalTracker
    {
        bool IsCompleted { get; }

        /// <summary>0 ile 1 arasi ilerleme, UI'daki bar icin.</summary>
        float Progress { get; }

        /// <summary>UI'da gosterilecek metin, ornegin "320 / 500".</summary>
        string ProgressText { get; }

        event Action Changed;

        /// <summary>Her damladan sonra olusan tepkilerle cagrilir.</summary>
        void OnReactions(IReadOnlyList<ReactionEvent> events);
    }
}
