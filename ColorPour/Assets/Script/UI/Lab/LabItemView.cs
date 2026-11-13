using BottleFill.Meta.Lab;
using UnityEngine;

namespace BottleFill.UI.Lab
{
    /// <summary>
    /// Laboratuvarda bir item'in sahnedeki gorseli. Item alininca
    /// ownedVisual'i, alinmadan once notOwnedVisual'i gosterir.
    /// </summary>
    public class LabItemView : MonoBehaviour
    {
        [SerializeField] private LabUpgradeDefinition upgrade;

        [Tooltip("Satin alininca gorunecek obje (bu objenin bir cocugu).")]
        [SerializeField] private GameObject ownedVisual;

        [Tooltip("Istege bagli: satin alinmadan once gorunecek obje (golge, insaat alani vb.).")]
        [SerializeField] private GameObject notOwnedVisual;

        public void Refresh(LabModel lab)
        {
            bool owned = upgrade != null && lab.IsOwned(upgrade);
            if (ownedVisual != null) ownedVisual.SetActive(owned);
            if (notOwnedVisual != null) notOwnedVisual.SetActive(!owned);
        }
    }
}
