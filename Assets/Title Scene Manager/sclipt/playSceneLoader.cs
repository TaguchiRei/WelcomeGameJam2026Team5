using UnityEngine;
using UnityEngine.SceneManagement;

public class playSceneLoader : MonoBehaviour
{
    [SerializeField]
    string sceneName;
    public void ButtonSystem()
    {
        SceneManager.LoadScene(sceneName);
    }
}
