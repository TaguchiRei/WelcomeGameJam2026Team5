using UnityEngine;
using UnityEngine.SceneManagement;

public class Back2Title : MonoBehaviour
{
    public float waitTime = 2f;
    private bool canReturn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canReturn = false;
        Invoke("EnableReturn", waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (canReturn && Input.GetKeyDown(KeyCode.Space))
        {
            ReturnToTitle();
        }
    }

    void EnableReturn()
    {
        canReturn = true;
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("TitleScene");
    }
}