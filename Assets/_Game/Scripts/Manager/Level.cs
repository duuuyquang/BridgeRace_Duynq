using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] NavMeshData navMeshData;
    private int index;

    public Transform finishPoint;
    public int Index {  get { return index; } set { index = value; } }

    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
    }
}
