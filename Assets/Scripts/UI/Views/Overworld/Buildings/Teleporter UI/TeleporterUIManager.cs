using System.Collections.Generic;
using System.Linq;
using Building.Buildings.Base_Classes;
using Building.Structures;
using Building.Systems;
using Scriptable_Object_Templates.Singletons;
using TMPro;
using UI.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.Buildings.Teleporter_UI
{
    public class TeleporterUIManager : MonoBehaviour, IUIElement
    {
        private readonly Dictionary<Teleporter, TeleporterTile> _teleporterTiles = new();
        
        private Teleporter _currentTeleporter, _openingTeleporter;

        [SerializeField] private TMP_Text errorText;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private Button teleportButton;
        [SerializeField] private TeleporterTile selectedTeleporterTile;
        [SerializeField] private GameObject teleporterTilePrefab;

        public void Initialize()
        {
            if (!BuildingManager.Instance.BuildingsByType.TryGetValue(BuildingType.Teleporter, out _))
            {
                return;
            }
            
            if (BuildingManager.Instance.BuildingsByType[BuildingType.Teleporter]
                    .Count(b => b.currentState == BuildingState.Constructed) < 2)
            {
                errorText.enabled = true;
                grid.enabled = false;
                selectedTeleporterTile.enabled = false;
                teleportButton.interactable = false;
            }
            else
            {
                errorText.enabled = false;
                grid.enabled = true;
                selectedTeleporterTile.enabled = true;
                teleportButton.interactable = true;
                
                GenerateTeleporterTiles();
                UpdateSelectedTeleporterTile(_teleporterTiles.Keys.ElementAt(0));
            }
        }

        public void UpdateElement()
        {
            if (BuildingManager.Instance.BuildingsByType[BuildingType.Teleporter]
                    .Count(b => b.currentState == BuildingState.Constructed) < 2)
            {
                errorText.enabled = true;
                grid.enabled = false;
                teleportButton.interactable = false;
            }
            else
            {
                errorText.enabled = false;
                grid.enabled = true;
                teleportButton.interactable = true;
                
                GenerateTeleporterTiles();

                var otherTeleporters = _teleporterTiles.Keys.ToList();
                otherTeleporters.Remove(_openingTeleporter);
                
                UpdateSelectedTeleporterTile(otherTeleporters.ElementAt(0));
            }
        }

        public void CloseElement()
        {
            
        }

        public void SetOpeningTeleporter(Teleporter teleporter)
        {
            _openingTeleporter = teleporter;
        }
        
        private void GenerateTeleporterTiles()
        {
            var teleporters = BuildingManager.Instance
                .BuildingsByType[BuildingType.Teleporter]
                .Where(b => b.currentState == BuildingState.Constructed)
                .Select(b => b.buildingComponent)
                .Cast<Teleporter>()
                .ToList();

            teleporters.Remove(_openingTeleporter);
            
            _teleporterTiles.Clear();
            
            foreach (var teleporter in teleporters)
            {
                CreateTeleporterTile(teleporter);
            }
        }

        private void CreateTeleporterTile(Teleporter teleporter)
        {
            var teleporterData = BuildingPrefabDictionary.Instance.dictionary[BuildingType.Teleporter];
            var teleporterObject = Instantiate(teleporterTilePrefab, grid.transform);
            var teleporterTile = teleporterObject.GetComponent<TeleporterTile>();

            teleporterTile.Icon = teleporterData.icon;
            teleporterTile.coordinates.SetText(teleporter.transform.position.ToString());
            
            _teleporterTiles[teleporter] = teleporterTile;
        }

        private void UpdateSelectedTeleporterTile(Teleporter teleporter)
        {
            var teleporterData = _teleporterTiles[teleporter];

            selectedTeleporterTile.Icon = teleporterData.Icon;
            selectedTeleporterTile.coordinates = teleporterData.coordinates;
        }
    }
}