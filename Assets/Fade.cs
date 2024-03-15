using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade: MonoBehaviour
{
    public float fadeDuration = 2f;
    public string endSceneName;
    public SideCube character;

    void Update()
    {
        if (character.end)
        {
            StartFade();
        }
    }

    void StartFade()
    {
        Invoke("LoadEndScene", fadeDuration);
    }

    void LoadEndScene()
    {
        SceneManager.LoadScene(endSceneName);
    }
}
