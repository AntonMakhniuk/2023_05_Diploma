using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Production.Crafting;
using Scriptable_Object_Templates.Crafting;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Recipe Dictionary", menuName = "GameData/RecipeDictionary")]
    public class RecipeDictionary : ScriptableSingleton<RecipeDictionary>
    {
        [SerializedDictionary("Recipe Difficulty", "Associated Recipe")]
        public SerializedDictionary<Difficulty, List<Recipe>> dictionary;
    }
}