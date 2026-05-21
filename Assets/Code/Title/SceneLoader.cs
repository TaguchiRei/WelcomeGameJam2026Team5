using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionDuration = 1f;   
    
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
            return;

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isTransitioning = true;

        transitionAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(transitionDuration);

        yield return SceneManager.LoadSceneAsync(sceneName);

        transitionAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(transitionDuration);

        isTransitioning = false;
    }
}