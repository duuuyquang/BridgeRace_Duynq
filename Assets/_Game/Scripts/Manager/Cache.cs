using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, Brick>      dictBridgeBrick = new Dictionary<Collider, Brick>();
    private static Dictionary<Collider, Stair>      dictBridgeStair = new Dictionary<Collider, Stair>();
    private static Dictionary<Collider, Stage>      dictBridgeStage = new Dictionary<Collider, Stage>();
    private static Dictionary<Collider, Character>  dictBridgeChar  = new Dictionary<Collider, Character>();

    public static Character GenCharacters(Collider collider)
    {
        if (!dictBridgeChar.ContainsKey(collider))
        {
            Character bridge = collider.GetComponent<Character>();
            dictBridgeChar.Add(collider, bridge);
        }

        return dictBridgeChar[collider];
    }

    public static Brick GenBricks(Collider collider)
    {
        if (!dictBridgeBrick.ContainsKey(collider))
        {
            Brick bridge = collider.GetComponent<Brick>();
            dictBridgeBrick.Add(collider, bridge);
        }

        return dictBridgeBrick[collider];
    }

    public static Stair GenStairs(Collider collider)
    {
        if (!dictBridgeStair.ContainsKey(collider))
        {
            Stair bridge = collider.GetComponent<Stair>();
            dictBridgeStair.Add(collider, bridge);
        }

        return dictBridgeStair[collider];
    }

    public static Stage GenStages(Collider collider)
    {
        if (!dictBridgeStage.ContainsKey(collider))
        {
            Stage bridge = collider.GetComponent<Stage>();
            dictBridgeStage.Add(collider, bridge);
        }

        return dictBridgeStage[collider];
    }
}
