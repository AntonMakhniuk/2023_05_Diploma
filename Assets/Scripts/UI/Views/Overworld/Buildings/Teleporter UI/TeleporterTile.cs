using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.Buildings.Teleporter_UI
{
    public class TeleporterTile : MonoBehaviour
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
        public TMP_Text coordinates;
    }
}