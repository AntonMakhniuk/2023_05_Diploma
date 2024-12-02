using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Scriptable_Object_Templates.Singletons
{
    [CreateAssetMenu(fileName = "Material Alpha Dictionary", menuName = "GameData/MaterialAlphaDictionary")]
    public class MaterialAlphaDictionary : ScriptableSingleton<MaterialAlphaDictionary>
    {
        [SerializedDictionary("Opaque Material", "Transparent Material")]
        public SerializedDictionary<Material, Material> opaqueToTransparentDictionary;

        public Material OpaqueToTransparent(Material material)
        {
            return opaqueToTransparentDictionary
                .FirstOrDefault(m => m.Key.name == material.name
                    .Replace(" (Instance)", "") && m.Key.shader == material.shader).Value;
        }
        
        public Material TransparentToOpaque(Material material)
        {
            return opaqueToTransparentDictionary
                .FirstOrDefault(m => m.Value.name == material.name
                    .Replace(" (Instance)", "") && m.Value.shader == material.shader).Key;
        }
    }
}