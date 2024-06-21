using System.Collections.Generic;
using UnityEngine;

public static class DataByType
{
    public const int COLOR_TYPE_START         = 1;
    public const int COLOR_TYPE_END           = 5;
    public const int COLOR_TYPE_ALL           = 6;

    public static List<Color> Colors = new List<Color>() { 
        Color.clear, 
        Color.magenta,
        Color.red,
        Color.yellow, 
        Color.green,
        Color.blue,
        Color.grey
    };
}