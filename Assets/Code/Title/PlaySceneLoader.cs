using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneLoader : MonoBehaviour
{
    public float _rateSecond;
    
    [SerializeField] string _sceneName;

    public void ButtonSystem()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadRate()
    {
        Invoke(nameof(ButtonSystem), _rateSecond);
    }
}