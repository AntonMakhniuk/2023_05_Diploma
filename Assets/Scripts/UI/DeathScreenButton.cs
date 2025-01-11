using UnityEngine.SceneManagement;
using UnityEngine;
using Environment.Scene_Management;

public class DeathScreenButton : MonoBehaviour
{
    public void NavigateToMainMenu()
    {
        LevelManager.ChangeScene(SceneType.MainMenu);
    }
}