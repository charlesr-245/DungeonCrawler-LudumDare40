using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    private int currentScene = 0;
    public GameObject canvas;
    public GameObject helpCanvas;
    public GameObject fadeCanvas;

    private static int numberOfManagers;

    private void Start()
    {
        if (numberOfManagers > 0)
        {
            Destroy(gameObject);
        }
        numberOfManagers++;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(id);
        currentScene = id;
    }

    public void Help()
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
            helpCanvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(true);
            helpCanvas.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
