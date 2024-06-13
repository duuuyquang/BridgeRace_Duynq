using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Level[] levels;
    public Level currentLevelObj;
    public int CurrentLevel => currentLevelObj.Index;

    [SerializeField] Player player;
    [SerializeField] List<Enemy> botPrefabs;
    private List<Enemy> bots = new List<Enemy>();

    public void Start()
    {
        OnLoadLevel(1);
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
        Vector3 leftPlayer = player.transform.position + Vector3.left * 2;
        Vector3 rightPlayer = player.transform.position + Vector3.right * 2;
        Vector3 spawnPos = player.transform.position;
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
        if (level > levels.Length-1)
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
