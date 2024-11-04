using System.Collections.Generic;
using Scriptable_Object_Templates.Crafting;

namespace Production.Crafting
{
    public class CraftingData
    {
        public readonly Recipe Recipe;
        public int CraftedQuantity;
        public const int StartingBonusModifier = 100;
        public int CurrentBonusModifier;
        public readonly Dictionary<BonusSource, int> BonusChangeLog = new();
        public bool ProductionFailed;

        public CraftingData(Recipe recipe, int craftedQuantity)
        {
            Recipe = recipe;
            CraftedQuantity = craftedQuantity;
            CurrentBonusModifier = StartingBonusModifier;
        }

        public void UpdateModifier(BonusSource source, int modifierDifference)
        {
            if (BonusChangeLog.ContainsKey(source))
            {
                BonusChangeLog[source] += modifierDifference;
            }
            else
            {
                BonusChangeLog[source] = modifierDifference;
            }
            
            CurrentBonusModifier += modifierDifference;
        }

        public void FailProduction()
        {
            ProductionFailed = true;
            
            UpdateModifier(BonusSource.GeneralFail, -CurrentBonusModifier);
        }

        public void FinaliseCraftingData()
        {
            CraftedQuantity *= CurrentBonusModifier / 100;
        }
    }

    public enum BonusSource
    {
        Morale, ResourceSpecific, GeneralFail
    }
}