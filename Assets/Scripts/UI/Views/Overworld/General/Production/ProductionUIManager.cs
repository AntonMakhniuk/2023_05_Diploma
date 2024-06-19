﻿using System.Collections.Generic;
using System.Linq;
using Scriptable_Object_Templates.Crafting;
using UI.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Production
{
    public class ProductionUIManager : MonoBehaviour, IUIElement
    {
        private readonly List<RecipeTile> _recipeTiles = new();
        private Recipe _selectedRecipe;
        
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private SelectedRecipeTile selectedRecipeTile;
        [SerializeField] private GameObject recipeTilePrefab;

        public void Initialize()
        {
            
        }

        public void UpdateElement()
        {
            GenerateRecipeTiles();
            UpdateSelectedRecipeTile(_recipeTiles.ElementAt(0));
        }

        public void CloseElement()
        {
            
        }
        
        private void GenerateRecipeTiles()
        {
            var recipes = new List<Recipe>(Resources.LoadAll<Recipe>("Recipes"));
            
            foreach (var recipe in recipes)
            {
                CreateRecipeTile(recipe);
            }
        }

        private void CreateRecipeTile(Recipe recipe)
        {
            Debug.Log(recipeTilePrefab);
            Debug.Log(grid);
            
            var recipeTileObject = Instantiate(recipeTilePrefab, grid.transform);
            var recipeTile = recipeTileObject.GetComponent<RecipeTile>();

            recipeTile.Icon = recipe.icon;
            recipeTile.recipeName.SetText(recipe.label);
            recipeTile.recipe = recipe;
            
            _recipeTiles.Add(recipeTile);
        }

        public void UpdateSelectedRecipeTile(RecipeTile recipeTile)
        {
            selectedRecipeTile.Icon = recipeTile.Icon;
            selectedRecipeTile.recipeName.SetText(recipeTile.recipeName.text);

            foreach (var kvp in recipeTile.recipe.resources)
            {
                selectedRecipeTile.RequiredResources.Add(kvp.resource, kvp.quantity);
            }

            selectedRecipeTile.GenerateRequirementsList();
        }
    }
}