using UnityEngine;
using UnityEngine.SceneManagement;

public class playSceneLoader : MonoBehaviour
{
    [SerializeField]
    string sceneName;
     public float rateSecond;
    public void ButtonSystem()
    {

        SceneManager.LoadScene(sceneName);

    }
    public void loadrate()
    {
        Invoke("ButtonSystem",rateSecond);
    }
}
