using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class SimplePool
{
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    //Init pool
    public static void Preload(GameUnit prefab, int amount, Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("PREFAB IS EMPTY !!! ");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.PoolType) || poolInstance[prefab.PoolType] == null)
        {
            Pool p = new Pool();
            p.Preload(prefab, amount, parent);
            poolInstance[prefab.PoolType] = p;
        }
    }

    //Take element out
    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + " IS NOT PRELOAD !!!");
            return null;
        }

        return poolInstance[poolType].Spawn(pos, rot) as T;
    }

    //Return element in
    public static void Despawn(GameUnit unit)
    {
        if (!poolInstance.ContainsKey(unit.PoolType))
        {
            Debug.LogError(unit.PoolType + "IS NOT PRELOAD !!!");
        }
        poolInstance[unit.PoolType].Despawn(unit);
    }

    //Collect element
    public static void Collect(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD !!!");
        }
        poolInstance[poolType].Collect();
    }

    public static void CollectAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    //Destroy 1 pool
    public static void Release(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD !!!");
        }
        poolInstance[poolType].Release();
    }

    public static void ReleaseAll()
    {
        foreach (var item in poolInstance.Values)
        {
            item.Release();
        }
    }
}

public class Pool
{
    Transform parent;
    GameUnit prefab;

    //list contains units in pool
    Queue<GameUnit> inactives = new Queue<GameUnit>();

    //list contains units being used 
    List<GameUnit> actives = new List<GameUnit>();

    //Init pool
    public void Preload(GameUnit prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < amount; i++)
        {
            inactives.Enqueue(GameObject.Instantiate(prefab, parent));
        }
    }

    //Take element out from pool
    public GameUnit Spawn(Vector3 position, Quaternion rot)
    {
        GameUnit unit;
        if (inactives.Count <= 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }

        unit.TF.SetPositionAndRotation(position, rot);
        actives.Add(unit);
        unit.gameObject.SetActive(true);

        return unit;
    }

    //Return element into pool
    public void Despawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            actives.Remove(unit);
            inactives.Enqueue(unit);
            unit.gameObject.SetActive(false);
        }
    }

    //Collect all elements being used into pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    //Destroy all elements
    public void Release()
    {
        Collect();
        while (inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}