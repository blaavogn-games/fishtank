using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSound : MonoBehaviour
{
    public static SceneSound instance;
    public string Scene;

    void Awake()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (instance == null || instance.Scene != scene && this != instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
            Scene = scene;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Scene)
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            Destroy(gameObject);
        }
    }
}
