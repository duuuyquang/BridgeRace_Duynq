using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Level[] levels;
    [SerializeField] Player player;
    [SerializeField] List<Enemy> botPrefabs;
    public int initLevel = 1;

    private List<Enemy> bots = new List<Enemy>();

    private Level currentLevelObj;
    public int CurrentLevel => currentLevelObj.Index;

    public int LevelMax => levels.Length - 1;

    public Transform CurrentFinishPoint => currentLevelObj.finishPoint;

    public void Start()
    {
        OnLoadLevel(initLevel);
        OnInit();
    }

    public void OnInit()
    {
        player.OnInit();
        OnInitBot();
    }

    public void OnInitBot()
    {
        int count = 1;
        Vector3 leftPlayer = player.TF.position + Vector3.left * 2;
        Vector3 rightPlayer = player.TF.position + Vector3.right * 2;
        Vector3 spawnPos = player.TF.position;
        
        foreach (Enemy enemy in botPrefabs) {
            if (count%2 == 1)
            {
                spawnPos += rightPlayer * count;
            } 
            else
            {
                spawnPos += leftPlayer * count;
            }
            Enemy temp = Instantiate(enemy, spawnPos, Quaternion.identity);
            bots.Add(temp);
            count++;
        }
    }

    public void OnReset()
    {
        player.OnDespawn();
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }

        bots.Clear();
        OnInitBot();
        SimplePool.CollectAll();
    }

    public void OnLoadLevel(int level)
    {
        if (level > LevelMax)
        {
            level = 1;
        }

        if (currentLevelObj != null)
        {
            Destroy(currentLevelObj.gameObject);
        }

        currentLevelObj = Instantiate(levels[level]);
        currentLevelObj.Index = level;
    }
}
