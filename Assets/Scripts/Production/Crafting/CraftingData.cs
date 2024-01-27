namespace Production.Crafting
{
    public class CraftingData
    {
        public readonly Recipe Recipe;
        public int CraftedQuantity;
        public float BonusModifier = 100f;

        public CraftingData(Recipe recipe, int craftedQuantity)
        {
            Recipe = recipe;
            CraftedQuantity = craftedQuantity;
        }
    }
}