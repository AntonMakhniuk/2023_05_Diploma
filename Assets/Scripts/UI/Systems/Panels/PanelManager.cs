using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Systems.Panels
{
    public class PanelManager : MonoBehaviour
    {
        [SerializedDictionary("Input Action", "Associated Panel")]
        public SerializedDictionary<InputActionReference, UIPanel> uiPanelsDict = new();

        public List<UIPanel> uiPanelsStatic = new();
        
        private static PanelManager _instance;

        [SerializeField] private UIPanel pausePanel;
        
        private PlayerInputActions _playerInputActions;
        private UIPanel _mainPanel;
        private readonly List<UIPanel> _childPanels = new();
        
        private readonly Dictionary<InputActionReference, Action<InputAction.CallbackContext>> _delegates = new();
        
        private void Awake()
        {
            _instance = this;
        }
        
        private void Start()
        {
            _playerInputActions = PlayerActions.InputActions;

            foreach (var inputActionReference in uiPanelsDict.Keys)
            {
                Action<InputAction.CallbackContext> action = _ => ToggleMainPanel(uiPanelsDict[inputActionReference]);
                _delegates[inputActionReference] = action;
                
                inputActionReference.action.performed += action;
                inputActionReference.action.Enable();
            }
            
            _playerInputActions.UI.CloseWindowOpenPause.performed += HandlePauseAction;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            
            foreach (var uiPanel in uiPanelsDict.Values)
            {
                uiPanel.Initialize();
            }

            foreach (var uiPanel in uiPanelsStatic)
            {
                uiPanel.Initialize();
                uiPanel.Open();
            }
        }

        public static void ToggleMainPanel(UIPanel panel)
        {
            Debug.Log(_instance._mainPanel);
            
            switch (_instance._mainPanel == null)
            {
                case true:
                {
                    _instance.OpenMainPanel(panel);
                    
                    return;
                }
                case false when _instance._mainPanel == panel:
                {
                    _instance.CloseMainPanel();
                    
                    return;
                }
                case false when _instance._mainPanel != panel:
                {
                    _instance.CloseMainPanel();
                    _instance.OpenMainPanel(panel);
                    
                    return;
                }
            }
        }

        public static void ToggleChildPanel(UIPanel panel)
        {
            switch (_instance._mainPanel == null)
            {
                case true:
                {
                    return;
                }
                case false when _instance._childPanels.Contains(panel):
                {
                    _instance.CloseChildPanel(panel);
                    
                    return;
                }
                case false when !_instance._mainPanel.childPanels.Contains(panel):
                {
                    return;
                }
                case false when _instance._mainPanel.childPanels.Contains(panel):
                {
                    _instance.OpenChildPanel(panel);
                    
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
                OpenMainPanel(_instance.pausePanel);
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
            foreach (var inputActionReference in uiPanelsDict.Keys)
            {
                if (_delegates.TryGetValue(inputActionReference, out var action))
                {
                    inputActionReference.action.performed -= action;
                }
            }

            if (_playerInputActions != null)
            {
                _playerInputActions.UI.CloseWindowOpenPause.performed -= HandlePauseAction;
            }

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}