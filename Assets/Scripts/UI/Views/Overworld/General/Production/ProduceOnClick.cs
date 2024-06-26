using Production.Crafting;
using Production.Systems;
using UI.Systems.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Production
{
    public class ProduceOnClick : MonoBehaviour
    {
        [SerializeField] private UIPanel parentPanel;
        [SerializeField] private Button parentButton;
        [SerializeField] private SelectedRecipeTile selectedRecipeTile;
        
        public void ProduceOne()
        {
            PanelManager.ToggleParentPanel(parentPanel);
            ProductionManager.StartProduction(new CraftingData(selectedRecipeTile.recipe, 1));
        }
    }
}