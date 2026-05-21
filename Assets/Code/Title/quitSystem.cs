using UnityEngine;

public class quitSystem : MonoBehaviour
{
    public void QuitSystem()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;

    }
}
