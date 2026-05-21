using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject startPanel;

    private bool gameStarted = false;
    void Start()
    {
        startPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.L))
        {
            StartGameNow();
        }
    }

    void StartGameNow()
    {
        gameStarted = true;

        startPanel.SetAcive(false);
        Time.timeScale = 1f;

        Debug.Log("Game Start!");
    }
}
