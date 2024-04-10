using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadMode { Add, Replace }

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Min(0)]
    [Tooltip("Just in case it's necessary to artifically increase loading times")]
    [SerializeField] private float forcedLoadDelay;

    [SerializeField] private int _currentSceneID;

    public float Progress { get; private set; }

    public event Action<bool> OnLoadIsInProgress;
    public event Action<float> OnProgressChanges;
    public event Action<int> OnSceneIDLoaded;


    private void Awake()
    {
        InitSingleton();
    }

    private void Start()
    {
        LoadLevel(1, LoadMode.Add);
    }


    private void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ReloadLevel()
    {
        LoadLevel(_currentSceneID, LoadMode.Replace);
    }

    public bool LoadLevel(int sceneID, LoadMode loadMode)
    {
        if (sceneID >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Something tried to access a level that doesn't exist within build settings.");

            return false;
        }

        StartCoroutine(LoadAsynchronously(sceneID, loadMode));

        return true;
    }

    public bool UnloadLevel(int sceneID)
    {
        if (sceneID == 0)
        {
            Debug.LogWarning("Cannot unload BootLoader");

            return false;
        }

        SceneManager.UnloadSceneAsync(sceneID);

        return true;
    }

    private IEnumerator LoadAsynchronously(int sceneID, LoadMode loadMode)
    {        
        Progress = 0f;
        OnProgressChanges?.Invoke(Progress);
        OnLoadIsInProgress?.Invoke(true);

        yield return new WaitForSeconds(forcedLoadDelay);

        if (loadMode == LoadMode.Replace)
        {
            UnloadLevel(_currentSceneID);
        }

        _currentSceneID = sceneID;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            Progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            Progress = Mathf.Round(Progress * 100f);
            OnProgressChanges?.Invoke(Progress);

            yield return null;
        }

        OnSceneIDLoaded?.Invoke(sceneID);
                
        //Progress = 0f;
        //OnProgressChanges?.Invoke(Progress);
        OnLoadIsInProgress?.Invoke(false);
    }
}