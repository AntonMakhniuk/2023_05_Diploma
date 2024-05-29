using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UI_Categories.Inventory
{
    public class ItemTile : MonoBehaviour
    {
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
        public TMP_Text quantity;
        [SerializeField] private Image renderedImage;
    }
}