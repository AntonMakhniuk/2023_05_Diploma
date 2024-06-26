using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Systems.Panels
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance;

        [SerializeField] private UIPanel pausePanel;
        
        private PlayerInputActions _playerInputActions;
        private UIPanel _mainPanel;
        private readonly List<UIPanel> _uiPanels = new();
        private readonly List<UIPanel> _childPanels = new();
        
        private readonly Dictionary<InputActionReference, Action<InputAction.CallbackContext>> _delegates = new();
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            _playerInputActions = PlayerActions.InputActions;
            
            if (Instance.pausePanel != null)
            {
                _playerInputActions.UI.CloseWindowOpenPause.performed += HandlePauseAction;
            }

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void SubscribePanel(UIPanel panel)
        {
            _uiPanels.Add(panel);
            
            foreach (var toggleBind in panel.toggleBinds)
            {
                Action<InputAction.CallbackContext> action = _ => ToggleParentPanel(panel);
                _delegates[toggleBind] = action;
                
                toggleBind.action.performed += action;
                toggleBind.action.Enable();
            }
            
            panel.Initialize();

            if (panel.type == UIPanel.PanelType.Static)
            {
                panel.Open();
            }
        }
        
        public static void ToggleParentPanel(UIPanel panel)
        {
            Debug.Log(Instance._mainPanel);
            
            switch (Instance._mainPanel == null)
            {
                case true:
                {
                    Instance.OpenMainPanel(panel);
                    
                    return;
                }
                case false when Instance._mainPanel == panel:
                {
                    Instance.CloseMainPanel();
                    
                    return;
                }
                case false when Instance._mainPanel != panel:
                {
                    Instance.CloseMainPanel();
                    Instance.OpenMainPanel(panel);
                    
                    return;
                }
            }
        }

        public static void ToggleChildPanel(UIPanel panel)
        {
            switch (Instance._mainPanel == null)
            {
                case true:
                {
                    return;
                }
                case false when Instance._childPanels.Contains(panel):
                {
                    Instance.CloseChildPanel(panel);
                    
                    return;
                }
                case false when !Instance._mainPanel.childPanels.Contains(panel):
                {
                    return;
                }
                case false when Instance._mainPanel.childPanels.Contains(panel):
                {
                    Instance.OpenChildPanel(panel);
                    
                    return;
                }
            }
        }
        
        private void HandlePauseAction(InputAction.CallbackContext _)
        {
            if (_mainPanel != null)
            {
                CloseMainPanel();
            }
            else
            {
                OpenMainPanel(Instance.pausePanel);
            }
        }

        private void OpenMainPanel(UIPanel panel)
        {
            if (_mainPanel != null)
            {
                return;
            }
            
            _mainPanel = panel;
            _mainPanel.Open();
        }
        
        private void OpenChildPanel(UIPanel panel)
        {
            if (_mainPanel == null)
            {
                return;
            }

            if (!_mainPanel.childPanels.Contains(panel))
            {
                return;
            }

            panel.Open();
            _childPanels.Add(panel);
        }
        
        private void CloseMainPanel()
        {
            if (_mainPanel == null)
            {
                return;
            }

            _mainPanel.Close();

            _mainPanel = null;
            _childPanels.Clear();
        }
        
        private void CloseChildPanel(UIPanel panel)
        {
            if (_mainPanel == null)
            {
                return;
            }

            if (!_mainPanel.childPanels.Contains(panel))
            {
                return;
            }

            panel.Close();
            _childPanels.Remove(panel);
        }
        
        private void OnSceneUnloaded(Scene _)
        {
            CloseMainPanel();
        }

        private void OnDestroy()
        {
            foreach (var toggleBind in _uiPanels.SelectMany(panel => panel.toggleBinds))
            {
                toggleBind.action.performed -= _delegates[toggleBind];
            }

            if (_playerInputActions != null)
            {
                _playerInputActions.UI.CloseWindowOpenPause.performed -= HandlePauseAction;
            }

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}