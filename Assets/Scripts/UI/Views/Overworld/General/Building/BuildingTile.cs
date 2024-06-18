using Building.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Building
{
    public class BuildingTile : MonoBehaviour
    {
        [SerializeField] private Image renderedImage;
        private Sprite _icon;
        public Sprite Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                renderedImage.sprite = _icon;
            }
        }
        public TMP_Text itemName;
        public BuildingTypeContainer typeContainer;
    }
}