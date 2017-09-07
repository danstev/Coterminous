using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{

    public void CloseGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void NewGame()
    {
        //SceneManager.LoadScene("Scene");
        Debug.Log("New Game");
    }

    public void SaveGame()
    {
        Debug.Log("Save Game");
    }

    public void LoadGame()
    {
        Debug.Log("Load Game");
    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
