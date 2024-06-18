using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Systems.Panels
{
    public class UIPanel : MonoBehaviour
    {
        public List<InputActionReference> toggleBinds = new();
        public List<UIPanel> childPanels = new();
        public PanelType type;
        
        [SerializeField] private List<GameObject> uiObjects = new();
        
        private readonly List<IUIElement> _uiElements = new();

        private void Awake()
        {
            PanelManager.Instance.SubscribePanel(this);
        }

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

            foreach (var panel in childPanels.Where(p => p.gameObject.activeSelf))
            {
                panel.Close();
            }
            
            foreach (var element in _uiElements)
            {
                element.CloseElement();
            }
        }

        public enum PanelType
        {
            Dynamic, Static
        }
    }
}