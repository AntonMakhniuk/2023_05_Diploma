using Environment.Global_Map.Entities;
using Environment.Scene_Management;
using TMPro;
using UI.Systems;
using UI.Systems.Miscellaneous;
using UnityEngine;

namespace UI.Views.Global_Map.General.Entry_Point_Data
{
    public class EntryPointUIManager : MonoBehaviour, IUIElement
    {
        [SerializeField] private TMP_Text locationNameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private SceneTypeContainer sceneTypeContainer;
        
        public void Initialize()
        {

        }

        public void UpdateElement()
        {
            
        }

        public void CloseElement()
        {

        }

        public void UpdateEntryPointData(EntryPoint entryPoint)
        {
            locationNameText.SetText(entryPoint.locationName);
            descriptionText.SetText(entryPoint.description);
            sceneTypeContainer.sceneType = entryPoint.sceneTypeContainer.sceneType;
        }
    }
}