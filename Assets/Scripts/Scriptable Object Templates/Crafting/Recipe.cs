using System.Collections.Generic;
using Production.Crafting;
using UnityEngine;

namespace Scriptable_Object_Templates.Crafting
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
    public class Recipe : ScriptableObject 
    {
        public Sprite icon;
        public string label;
        public DifficultyConfig difficultyConfig;
        public List<ResourceQuantityAssociation> resources;
    }
}