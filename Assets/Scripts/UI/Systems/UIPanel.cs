using System.Collections.Generic;
using UnityEngine;

namespace UI.Systems
{
    public class UIPanel : MonoBehaviour
    {
        public PanelType panelType;
        
        [SerializeField] private List<GameObject> uiObjects = new();
        
        private readonly List<IUIElement> _uiElements = new();
        
        public void Initialize()
        {
            foreach (var elementObject in uiObjects)
            {
                if (elementObject.TryGetComponent(out IUIElement element))
                {
                    _uiElements.Add(element);
                    element.Initialize();
                }
            }
            
            gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            
            foreach (var element in _uiElements)
            {
                element.UpdateElement();
            }
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            
            foreach (var element in _uiElements)
            {
                element.CloseElement();
            }
        }
    }

    public enum PanelType
    {
        Inventory, Journal, Map, Production, Wagons, Building, Pause, Upgrades
    }
}