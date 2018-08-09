using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public SceneAsset[] SceneFiles;
    public bool loadScenesTimer = true;
    public int currentActiveScene = -1;
    private float numberOfScenes = 5;
    private float secondsToLoadScene = 7;

    // Use this for initialization
    void Start ()
    {
        LoadScene(1, loadScenesTimer);
    }
	
    public void CallLoadScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadNextScene(loadScenesTimer));
    }

    public IEnumerator LoadNextScene(bool loadSceneTimer = false)
    {
        yield return UnloadScene(currentActiveScene);
        if (currentActiveScene + 1 > numberOfScenes)
            currentActiveScene = 0;
        LoadScene(currentActiveScene + 1, loadSceneTimer);
    }

    IEnumerator LoadSceneTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        CallLoadScene();
    }

    AsyncOperation UnloadScene(int scene)
    {
        if(scene <= numberOfScenes)
            return SceneManager.UnloadSceneAsync(scene);
        return null;
    }

	void LoadScene(int scene, bool loadSceneTimer = false)
    {
        if (scene <= numberOfScenes)
            SceneManager.LoadSceneAsync(scene,LoadSceneMode.Additive);
        currentActiveScene = scene;
        if (loadSceneTimer)
            StartCoroutine(LoadSceneTimer(secondsToLoadScene));
    }
}

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    SceneLoader myTarget;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myTarget = (SceneLoader)target;
        if (GUILayout.Button("Load Next Scene"))
        {
            myTarget.CallLoadScene();
        }
    }
}