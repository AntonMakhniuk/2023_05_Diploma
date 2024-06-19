using Production.Crafting;
using Production.Systems;
using Scriptable_Object_Templates.Crafting;
using UnityEngine;
using UnityEngine.UI;

public class AssignCraftToButton : MonoBehaviour
{
    public Recipe associatedRecipe;
    public Button associatedButton;
    public ProductionManager associatedManager;
    
    // Start is called before the first frame update
    void Start()
    {
        associatedButton.onClick.AddListener(CraftRecipe);
    }

    private void CraftRecipe()
    {
        associatedManager.StartProduction(new CraftingData(associatedRecipe, 1));
    }
}
