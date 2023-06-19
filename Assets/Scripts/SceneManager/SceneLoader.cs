using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool _shouldStartHost;
    private bool _isMultiplayer;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "MainScene")
        {
            _shouldStartHost = true;
        }

        if (sceneName == "Multiplayer")
        {
            _isMultiplayer = true;
            sceneName = "MainScene";
        }
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_shouldStartHost && scene.name == "MainScene")
        {
            NetworkManager.Singleton.StartHost();
            _shouldStartHost = false;
        } 
        if (_isMultiplayer)
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}