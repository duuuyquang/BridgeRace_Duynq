using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] Brick brickPrefab;
    [SerializeField] Transform spawnStartPos;

    private List<int> cachedColorTypeList = new List<int>();

    private int totalEach;

    public int rowNum;
    public int colNum;
    public int TotalBrickEachType => totalEach;

    void Awake()
    {
        OnInit();
    }

    private void OnInit()
    {
        PreloadTypeListToSpawn();
    }

    public List<Vector3> SpawnBricksByType(ColorType type) {
        List<Vector3> cachedPosList = new List<Vector3>();
        int count = 0;
        for (int i = 1; i <= colNum; i++)
        {
            for (int k = 0; k < rowNum; k++)
            {
                if (cachedColorTypeList[count++] == (int)type)
                {
                    Vector3 spawnPos = spawnStartPos.position + Vector3.right * k * 1.5f + Vector3.back * i * 1.5f;
                    cachedPosList.Add(spawnPos);
                    SpawnEachBrickByType(type, spawnPos);
                }
            }
        }

        return cachedPosList;
    }

    public void SpawnEachBrickByType(ColorType type, Vector3 spawnPos)
    {
        //Brick temp = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
        //temp.OnInit(type);
        //temp.transform.SetParent(spawnStartPos);

        Brick b = SimplePool.Spawn<Brick>(PoolType.Brick, spawnPos, Quaternion.identity);
        b.OnInit(type);
    }

    public List<int> PreloadTypeListToSpawn()
    {
        if (cachedColorTypeList.Count <= 0)
        {
            List<int> list = new List<int>();

            int totalType = LevelManager.Instance.TotalCharacters;
            totalEach = (int)Mathf.Ceil((float) rowNum * colNum / totalType);

            List<int> countHolder = new List<int>();
            for (int i = 0; i <= totalType; i++)
            {
                countHolder.Add(totalEach);
            }

            for (int i = DataByType.COLOR_TYPE_START; i <= totalType; i++)
            {
                while (countHolder[i]-- > 0)
                {
                    list.Add(i);
                }
            }

            cachedColorTypeList = Utilities.ShuffleList(list);
        }
        return cachedColorTypeList;
    }
}
