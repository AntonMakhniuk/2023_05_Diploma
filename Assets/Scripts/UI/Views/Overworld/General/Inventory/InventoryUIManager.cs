using System.Collections.Generic;
using Player.Inventory;
using Scriptable_Object_Templates.Resources;
using Scriptable_Object_Templates.Systems.Mining.Resource_Data;
using TMPro;
using UI.Systems;
using UI.Systems.Miscellaneous;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Overworld.General.Inventory
{
    public class InventoryUIManager : MonoBehaviour, IUIElement
    {
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private GameObject itemTilePrefab;

        private readonly Dictionary<ItemBase, ItemTile> _itemTileDictionary = new();

        private static List<ItemStack> Items => InventoryManager.Instance.GetAllItems();
        
        public void Initialize()
        {
            if (Items.Count == 0)
            {
                errorText.gameObject.SetActive(true);
            }
            else
            {
                errorText.gameObject.SetActive(false);
                
                GenerateItemTiles();   
            }
        }

        public void UpdateElement()
        {            
            if (Items.Count == 0)
            {
                errorText.gameObject.SetActive(true);
            }
            else
            {
                errorText.gameObject.SetActive(false);
                
                foreach (var stack in Items)
                {
                    if (_itemTileDictionary.TryGetValue(stack.item, out var itemTile))
                    {
                        itemTile.quantity.SetText($"{stack.quantity:N2}");
                    }
                    else
                    {
                        _itemTileDictionary.Add(stack.item, CreateItemTile(stack));
                    }
                }    
            }
        }

        public void CloseElement()
        {
            
        }

        private void GenerateItemTiles()
        {
            foreach (var stack in Items)
            {
                _itemTileDictionary.Add(stack.item, CreateItemTile(stack));
            }
        }

        private ItemTile CreateItemTile(ItemStack stack)
        {
            var itemTileObject = Instantiate(itemTilePrefab, grid.transform);
            var itemTile = itemTileObject.GetComponent<ItemTile>();

            itemTile.Icon = stack.item.icon;
            itemTile.itemName.SetText(stack.item.name);
            itemTile.quantity.SetText($"{stack.quantity:N2}");

            return itemTile;
        }
    }
}