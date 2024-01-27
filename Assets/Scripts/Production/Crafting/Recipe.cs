using System.Collections.Generic;
using UnityEngine;

namespace Production.Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
    public class Recipe : ScriptableObject
    {
        public Difficulty difficulty;
        public List<ResourceQuantityAssociation> resources;
    }
}