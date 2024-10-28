using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scriptable_Object_Templates.Singletons
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                var operation = Addressables.LoadAssetAsync<T>(typeof(T).Name);
                
                _instance = operation.WaitForCompletion();
                
                return _instance;
            }
        }
    }
}