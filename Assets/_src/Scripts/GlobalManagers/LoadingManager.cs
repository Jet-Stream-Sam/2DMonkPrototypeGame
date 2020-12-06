using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Camera tempCamera;
    private FadeInNOut tweenRef;
    private bool isReadyingToLoad = false;
    private string loadedScene;
    private string sceneToLoad;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private void Awake()
    {
        tweenRef = loadingScreen.GetComponentInChildren<FadeInNOut>();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        #endregion
    }

    private void Start()
    {
        tweenRef.hasFadeInAnimationFinished += LoadGame;
        tweenRef.hasFadeOutAnimationFinished += DeactivateLoadingScreen;
    }

    public void InitiateLoad(string sceneLoadedName, string sceneToLoadName)
    {
        if (isReadyingToLoad)
            return;
        loadedScene = sceneLoadedName;
        sceneToLoad = sceneToLoadName;

        PausingManager.canPause = false;
        loadingScreen.SetActive(true);
        tweenRef.FadeIn();
        isReadyingToLoad = true;

    }
    private void LoadGame()
    {
        tempCamera.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(loadedScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));

        StartCoroutine(LoadingScenesProgress());
    }

    private void DeactivateLoadingScreen()
    {
        loadingScreen.SetActive(false);
        isReadyingToLoad = false;
        PausingManager.canPause = true;
        
    }

    public IEnumerator LoadingScenesProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
        PausingManager pManager = GetComponent<PausingManager>();
        pManager.PauseReset();
        tempCamera.gameObject.SetActive(false);
        tweenRef.FadeOut();
        scenesLoading.Clear();
    }
}
