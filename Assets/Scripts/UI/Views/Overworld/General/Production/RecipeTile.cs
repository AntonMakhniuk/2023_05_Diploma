using Scriptable_Object_Templates.Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Production
{
    public class RecipeTile : MonoBehaviour
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
        public TMP_Text recipeName;
        public Recipe recipe;
    }
}