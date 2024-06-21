using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    Clear = 0,
    Magenta = 1,
    Red = 2,
    Yellow = 3,
    Green = 4,
    Blue = 5,
    Grey = 6
}

[CreateAssetMenu(menuName = "ColorDataSO")]

public class ColorDataSO : ScriptableObject
{
    [SerializeField] Material[] materials;

    public Material GetMat(ColorType index)
    {
        return materials[(int)index];
    }
}
