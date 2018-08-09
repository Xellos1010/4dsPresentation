using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class LoadNextPrefab : MonoBehaviour {
    
    public int activatedPrefab = -1;
    public float timerLoad = 5;

    private void Start()
    {
        StartCoroutine(LoadNextPrefabOnTimer(timerLoad));
    }

    private IEnumerator LoadNextPrefabOnTimer(float v)
    {
        yield return new WaitForSeconds(v);
        NextPrefab(true);
    }

    internal void NextPrefab(bool loadNextPrefabTimer = false)
    {
        StopAllCoroutines();
        activatedPrefab = activatedPrefab + 1 < transform.childCount? activatedPrefab + 1 : 0;
        ActivateChild(activatedPrefab);
        if(loadNextPrefabTimer)
            StartCoroutine(LoadNextPrefabOnTimer(timerLoad));
    }

    internal void ActivateChild(int v)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == v)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
        activatedPrefab = v;
    }
}
[CustomEditor(typeof(LoadNextPrefab))]
public class LoadNextPrefabEditor:Editor
{
    LoadNextPrefab myTarget;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myTarget = (LoadNextPrefab)target;
        if (GUILayout.Button("Activate First Child"))
        {
            myTarget.ActivateChild(0);
        }
        if (GUILayout.Button("Load Next Prefab"))
        {
            myTarget.NextPrefab();
        }
    }
}