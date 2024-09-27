using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARLocation.Utils
{
    public class SelectScene : MonoBehaviour
    {

        // Use this for initialization

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LogCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log("Current active scene: " + currentScene.name);
        }
    }
}
