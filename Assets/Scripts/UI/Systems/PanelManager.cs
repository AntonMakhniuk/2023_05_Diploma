using AYellowpaper.SerializedCollections;
using Player;
using UnityEngine;

namespace UI.Systems
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance;
        
        [SerializedDictionary("Panel Type", "Associated Panel")]
        public SerializedDictionary<PanelType, UIPanel> uiPanelsDict;
        
        private PlayerInputActions _playerInputActions;
        private UIPanel _currentPanel;
        
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
            
            _playerInputActions.UI.ToggleInventory.performed += _ => TogglePanel(PanelType.Inventory);
            _playerInputActions.UI.ToggleWagons.performed += _ => TogglePanel(PanelType.Wagons);
            _playerInputActions.UI.ToggleProduction.performed += _ => TogglePanel(PanelType.Production);
            _playerInputActions.UI.ToggleJournal.performed += _ => TogglePanel(PanelType.Journal);
            _playerInputActions.UI.ToggleMap.performed += _ => TogglePanel(PanelType.Map);
            _playerInputActions.UI.ToggleBuilding.performed += _ => TogglePanel(PanelType.Building);
            
            _playerInputActions.UI.CloseWindowOpenPause.performed += _ => HandlePauseAction();
            
            //TODO: Add interactable logic
            //_playerInputActions.UI.InteractWithObject.performed += _ => ...;

            foreach (var uiPanel in uiPanelsDict.Values)
            {
                uiPanel.Initialize();
            }

            // Set so that the value isn't null on start, doesn't actually mean anything
            _currentPanel = uiPanelsDict[PanelType.Wagons];
        }
        
        public delegate void OnPanelChangedHandler(PanelType panelType);

        public event OnPanelChangedHandler OnPanelOpened, OnPanelClosed;

        private void TogglePanel(PanelType panelType)
        {
            if (_currentPanel.gameObject.activeSelf && _currentPanel == uiPanelsDict[panelType])
            {
                ClosePanel();
                
                return;
            }

            if (_currentPanel.gameObject.activeSelf)
            {
                ClosePanel();
            }
            
            OpenPanel(panelType);
        }


        private void HandlePauseAction()
        {
            if (_currentPanel.gameObject.activeSelf)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel(PanelType.Pause);
            }
        }

        private void OpenPanel(PanelType panelType)
        {
            if (_currentPanel.gameObject.activeSelf)
            {
                return;
            }
            
            _currentPanel = uiPanelsDict[panelType];
            _currentPanel.Open();
            
            OnPanelOpened?.Invoke(_currentPanel.panelType);
        }
        
        private void ClosePanel()
        {
            if (_currentPanel.gameObject.activeSelf == false)
            {
                return;
            }
            
            _currentPanel.Close();
                
            OnPanelClosed?.Invoke(_currentPanel.panelType);
        }
    }
}