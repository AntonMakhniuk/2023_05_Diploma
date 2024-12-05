using System.Globalization;
using NaughtyAttributes;
using Player.Ship.Tools.Marker;
using TMPro;
using UI.Systems.Miscellaneous;
using UnityEngine;

namespace UI.Billboards
{
    public class MarkerIcon : BaseBillboard
    {
        [Foldout("Visualisation Data")] [SerializeField]
        private SpriteRenderer resourceSprite;
        [Foldout("Visualisation Data")] [SerializeField]
        private TMP_Text resourceNameText;
        [Foldout("Visualisation Data")] [SerializeField]
        private TMP_Text quantityText;
        [Foldout("Visualisation Data")] [SerializeField]
        private TMP_Text distanceText;

        [Foldout("Resource Data")] [SerializeField]
        private Collectable resourceData;

         protected override void Start()
        {
            base.Start();
            
            resourceSprite.sprite = resourceData.resource.icon;
            resourceNameText.SetText(resourceData.resource.name);
            quantityText.SetText(resourceData.AmountContained.ToString(CultureInfo.InvariantCulture));
            distanceText.SetText(DistanceFromPlayer.ToString("N2") + "m");
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            
            distanceText.SetText(DistanceFromPlayer.ToString("N2") + "m");
        }
    }
}