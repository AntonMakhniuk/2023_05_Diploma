namespace Production.Crafting
{
    public class CraftingData
    {
        public Recipe Recipe;
        public int CraftedQuantity;

        private float _bonusModifier = 100f;

        public CraftingData(Recipe recipe, int craftedQuantity)
        {
            Recipe = recipe;
            CraftedQuantity = craftedQuantity;
        }
    }
}