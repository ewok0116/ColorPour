using UnityEngine;

namespace BottleFill.Data
{
    /// <summary>Hangi tepkinin kac puan verdigi. Dengeyi koda dokunmadan ayarlamak icin.</summary>
    [CreateAssetMenu(fileName = "ScoringConfig", menuName = "Bottle Fill/Scoring Config")]
    public class ScoringConfig : ScriptableObject
    {
        [Min(0)] public int pointsPerEvaporate = 10;
        [Min(0)] public int pointsPerMix = 0;
        [Min(0)] public int pointsPerExplosion = 30;
        [Min(0)] public int pointsPerIceBreak = 20;
    }
}
