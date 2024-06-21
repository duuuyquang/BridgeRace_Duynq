using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static List<T> ShuffleList<T>(List<T> list)
    {
        List<T> newList = new List<T>();
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count);
            newList.Add(list[index]);
            list.RemoveAt(index);
        }
        return newList;
    }
}
