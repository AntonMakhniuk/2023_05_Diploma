using System.Collections.Generic;
using Scriptable_Object_Templates.Resources;
using TMPro;

namespace UI.Views.Overworld.General.Production
{
    public class SelectedRecipeTile : RecipeTile
    {
        public readonly Dictionary<Resource, float> RequiredResources = new();
        public TMP_Text requirementsText;
        
        public void GenerateRequirementsList()
        {
            requirementsText.SetText("");
            
            foreach (var kvp in RequiredResources)
            {
                requirementsText.SetText(requirementsText.text +
                                         $"{kvp.Key.label}: {kvp.Value}\n");
            }
        }
    }
}