using System.Collections.Generic;
using System.Linq;
using Player.Ship;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Scene_Management
{
    public class PlayerShipStateManager : MonoBehaviour
    {
        [SerializeField] private List<SceneType> scenesNoShip;

        private void Start()
        {
            SceneManager.sceneLoaded += HandleSceneChanged;
        }

        private void HandleSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
        {
            var sceneType = LevelManager.Instance.scenesDict.FirstOrDefault(i => 
                i.Value.Contains(scene.name)).Key;

            PlayerShip.Instance.gameObject.SetActive(!scenesNoShip.Contains(sceneType));
        }
    }
}