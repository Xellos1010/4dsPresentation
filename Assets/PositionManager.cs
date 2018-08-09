using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PositionManager : MonoBehaviour {
    public Vector3[] Positions;
    public Quaternion[] Rotations;
    public bool loadPositionsOnTimer = true;
    public float waitToLoadPosition = 5;

    public int CurrentPosition = -1;

    private void Start()
    {
        LoadNextPosition(0);
    }

    private IEnumerator LoadNextPosition()
    {
        yield return new WaitForSeconds(waitToLoadPosition);
        LoadNextPosition(CurrentPosition+1);
    }

    private void LoadNextPosition(int v, bool overrideTimer = false)
    {
        if (v >= Positions.Length)
            v = 0;
        LoadPosition(v);
        if (loadPositionsOnTimer && !overrideTimer)
            StartCoroutine(LoadNextPosition());
        if (overrideTimer)
            StopAllCoroutines();
    }

    public void LoadNextPosition(bool overrideTimer = true)
    {
        LoadNextPosition(CurrentPosition + 1,overrideTimer);
    }

    public void StartLoadPositionTimer()
    {
        StartCoroutine(LoadNextPosition());
    }

    internal void SetPosition(int i)
    {
        if (Positions.Length < i)
            Positions = new Vector3[i];
        Positions[i] = transform.position;
        Rotations[i] = transform.rotation;
    }

    internal void LoadPosition(int i)
    {
        transform.position = Positions[i];
        transform.rotation = Rotations[i];
        CurrentPosition = i;
    }

    internal void ResetPositions()
    {
        Positions = new Vector3[0];
        Rotations = new Quaternion[0];
    }
}

[CustomEditor(typeof(PositionManager))]
public class PositionManagerEditor : Editor
{
    PositionManager myTarget;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myTarget = (PositionManager)target;
        for (int i = 0; i < myTarget.Positions.Length; i++)
        {
            if (GUILayout.Button("Set Position " + i))
            {
                myTarget.SetPosition(i);
            }
            if (GUILayout.Button("Load Position " + i))
            {
                myTarget.LoadPosition(i);
            }
        }
        if (GUILayout.Button("Next Position"))
        {
            myTarget.LoadNextPosition();
        }
        if (GUILayout.Button("Reset Positions"))
        {
            myTarget.ResetPositions();
        }

    }
}
