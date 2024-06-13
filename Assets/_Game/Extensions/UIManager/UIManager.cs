using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();

    [SerializeField] Transform parent;

    private void Awake()
    {
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }

    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();
        return canvas;
    }

    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }

    public void CloseUIDirectly<T>() where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasActives[typeof(T)].CloseDirectly();
        }
    }

    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null;
    }

    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }

    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActives[typeof(T)] = canvas;
        }

        return canvasActives[typeof(T)] as T;
    }

    public T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    public void CloseAll()
    {
        foreach (var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
