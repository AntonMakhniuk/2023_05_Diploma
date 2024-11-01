﻿using Scriptable_Object_Templates.Building;
using Scriptable_Object_Templates.Singletons;
using UI.Systems.Miscellaneous;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Building
{
    public class BuildingUIManager: MonoBehaviour, IUIElement
    {
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private GameObject buildingTilePrefab;
        
        public void Initialize()
        {
            GenerateBuildingTiles();
        }
        
        public void UpdateElement()
        {

        }

        public void CloseElement()
        {

        }

        private void GenerateBuildingTiles()
        {
            foreach (var kvp in BuildingPrefabDictionary.Instance.dictionary)
            {
                CreateBuildingTile(kvp.Value);
            }
        }

        private void CreateBuildingTile(BuildingData data)
        {
            Debug.Log(buildingTilePrefab);
            Debug.Log(grid);
            
            var buildingTileObject = Instantiate(buildingTilePrefab, grid.transform);
            var buildingTile = buildingTileObject.GetComponent<BuildingTile>();
            
            buildingTile.Icon = data.icon;
            buildingTile.itemName.SetText(data.label);
            buildingTile.typeContainer.buildingType = data.type;
        }
    }
}