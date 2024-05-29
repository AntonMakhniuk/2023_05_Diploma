using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Systems.Panels
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance;
        
        [SerializedDictionary("Panel Type", "Associated Panel")]
        public SerializedDictionary<PanelType, UIPanel> uiPanelsDict = new();
        
        private PlayerInputActions _playerInputActions;
        private UIPanel _mainPanel;
        private readonly List<UIPanel> _childPanels = new();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            _playerInputActions = PlayerActions.InputActions;
            
            _playerInputActions.UI.ToggleInventory.performed += _ => ToggleMainPanel(PanelType.Inventory);
            _playerInputActions.UI.ToggleWagons.performed += _ => ToggleMainPanel(PanelType.Wagons);
            _playerInputActions.UI.ToggleProduction.performed += _ => ToggleMainPanel(PanelType.Production);
            _playerInputActions.UI.ToggleJournal.performed += _ => ToggleMainPanel(PanelType.Journal);
            _playerInputActions.UI.ToggleMap.performed += _ => ToggleMainPanel(PanelType.Map);
            _playerInputActions.UI.ToggleBuilding.performed += _ => ToggleMainPanel(PanelType.Building);
            _playerInputActions.UI.ToggleUpgrades.performed += _ => ToggleMainPanel(PanelType.Upgrades);
            
            _playerInputActions.UI.CloseWindowOpenPause.performed += _ => HandlePauseAction();

            SceneManager.sceneUnloaded += _ => CloseMainPanel();
            
            //TODO: Add interactable logic
            //_playerInputActions.UI.InteractWithObject.performed += _ => ...;

            foreach (var uiPanel in uiPanelsDict.Values)
            {
                uiPanel.Initialize();
            }
        }
        
        public delegate void OnPanelChangedHandler(PanelType panelType);

        public event OnPanelChangedHandler OnMainPanelOpened, OnMainPanelClosed;

        public static void ToggleMainPanel(PanelType panelType)
        {
            switch (Instance._mainPanel == null)
            {
                case true:
                {
                    Instance.OpenMainPanel(panelType);
                    
                    return;
                }
                case false when Instance._mainPanel == Instance.uiPanelsDict[panelType]:
                {
                    Instance.CloseMainPanel();
                    
                    return;
                }
                case false when Instance._mainPanel != Instance.uiPanelsDict[panelType]:
                {
                    Instance.CloseMainPanel();
                    Instance.OpenMainPanel(panelType);
                    
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
                    Debug.Log(1);
                    
                    return;
                }
                case false when Instance._childPanels.Contains(panel):
                {
                    Instance.CloseChildPanel(panel);
                    
                    return;
                }
                case false when !Instance._mainPanel.childPanels.Contains(panel):
                {
                    Debug.Log(3);
                    
                    return;
                }
                case false when Instance._mainPanel.childPanels.Contains(panel):
                {
                    Debug.Log(4);
                    
                    Instance.OpenChildPanel(panel);
                    
                    return;
                }
            }
        }
        
        private void HandlePauseAction()
        {
            if (_mainPanel != null)
            {
                CloseMainPanel();
            }
            else
            {
                OpenMainPanel(PanelType.Pause);
            }
        }

        private void OpenMainPanel(PanelType panelType)
        {
            if (_mainPanel != null)
            {
                return;
            }
            
            _mainPanel = uiPanelsDict[panelType];
            _mainPanel.Open();
            
            OnMainPanelOpened?.Invoke(_mainPanel.panelType);
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
                
            OnMainPanelClosed?.Invoke(_mainPanel.panelType);

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
    }
}