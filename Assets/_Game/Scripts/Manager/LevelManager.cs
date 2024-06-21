using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Level[] levels;
    [SerializeField] Player player;
    [SerializeField] Enemy botPrefab;

    private List<ColorType> availableColors = new List<ColorType>();
    private List<ColorType> shuffledColors = new List<ColorType>();
    private List<Enemy> bots = new List<Enemy>();
    private Level currentLevelObj;

    public int initLevel = 1;

    public int CurrentLevel => currentLevelObj.Index;

    public int LevelMax => levels.Length - 1;

    public int TotalCharacters => availableColors.Count;

    public Transform CurrentFinishPoint => currentLevelObj.finishPoint;

    public void Awake()
    {
        PreloadAvailableColors();
    }

    public void Start()
    {
        OnLoadLevel(initLevel);
        OnInit();
    }

    public void OnInit()
    {
        ShuffleColors();
        OnInitPlayer();
        OnInitBot();
    }

    public void OnInitBot()
    {
        int count = 1;
        Vector3 leftPlayer = player.TF.position + Vector3.left * 2;
        Vector3 rightPlayer = player.TF.position + Vector3.right * 2;
        Vector3 spawnPos = player.TF.position;

        foreach (ColorType color in shuffledColors) {
            if (count%2 == 1)
            {
                spawnPos += rightPlayer * count;
            }
            else
            {
                spawnPos += leftPlayer * count;
            }

            botPrefab.ColorType = color;
            Enemy temp = Instantiate(botPrefab, spawnPos, Quaternion.identity);
            bots.Add(temp);
            count++;
        }
    }

    public void PreloadAvailableColors()
    {
        if (availableColors.Count <= 0)
        {
            foreach (ColorType type in (ColorType[])Enum.GetValues(typeof(ColorType)))
            {
                if (type == ColorType.Clear || type == ColorType.Grey)
                {
                    continue;
                }
                availableColors.Add(type);
            }
        }
    }

    public void ShuffleColors()
    {
        shuffledColors.Clear();
        shuffledColors = Utilities.ShuffleList(new List<ColorType>(availableColors));
    }

    public void OnInitPlayer()
    {
        ColorType type = shuffledColors[shuffledColors.Count - 1];
        player.ColorType = type;
        shuffledColors.Remove(type);
        player.OnInit();
    }

    public void OnReset()
    {
        player.OnDespawn();
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }
        bots.Clear();
        SimplePool.CollectAll();

        OnInit();
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
