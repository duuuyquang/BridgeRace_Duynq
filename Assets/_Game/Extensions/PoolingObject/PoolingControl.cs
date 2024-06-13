using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;

    private void Awake()
    {
        // Load from resources
        GameUnit[] gameUnits = Resources.LoadAll<GameUnit>("Pool/");
        for (int i = 0; i < gameUnits.Length; i++)
        {
            SimplePool.Preload(gameUnits[i], 0, new GameObject(gameUnits[i].name).transform);
        }

        // Load from list
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            SimplePool.Preload(poolAmounts[i].prefab, poolAmounts[i].amount, poolAmounts[i].parent);
        }
    }
}

public enum PoolType
{
    Brick = 0,
    CharacterBrick = 1,
}

[System.Serializable]

public class PoolAmount
{
    public GameUnit prefab;
    public Transform parent;
    public int amount;
}