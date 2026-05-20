using UnityEngine;
using UnityEngine.SceneManagement;

public class playSceneLoader : MonoBehaviour
{
    [SerializeField]
    string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonSystem()
    {
        SceneManager.LoadScene(sceneName);
    }
}
