public static class TagManager
{
    public static string PLAYER = "Player";
    public static string ENEMY  = "Enemy";
    public static string BRICK  = "Brick";
    public static string STAIR  = "Stair";
    public static string STAGE  = "Stage";
    public static string FINISH = "Finish";
    public static string DOOR   = "Door";

    public static bool Compare(string tag1, string tag2)
    {
        return tag1.CompareTo(tag2) == 0;
    }
}
