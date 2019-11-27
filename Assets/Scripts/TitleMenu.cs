using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public string sceneName;
    public float loadAfter;
    protected AsyncOperation operation;

    public void Start()
    {
        Invoke("LoadTheScene", loadAfter);
    }

    public void LoadTheScene()
    {
        if (operation != null)
            return;
        operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void Update()
    {
        if (operation != null)
        {
            if (operation.isDone)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Title");
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown)
        {
            LoadTheScene();
        }
    }
}
