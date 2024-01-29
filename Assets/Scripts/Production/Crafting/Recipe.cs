using System.Collections.Generic;
using UnityEngine;

namespace Production.Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
    public class Recipe : ScriptableObject {
        public string label;
        public DifficultyConfig difficultyConfig;
        public List<ResourceQuantityAssociation> resources;
    }
}