using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameManager = new GameManager();
        gameManager.Start();
    }

    private void Update()
    {
        gameManager.Update();
    }

    private void OnDestroy()
    {
        gameManager.OnDestroy();
    }

    private void OnApplicationFocus(bool focus)
    {

    }
    private void OnApplicationPause(bool pause)
    {

    }
    private void OnApplicationQuit()
    {

    }
}
