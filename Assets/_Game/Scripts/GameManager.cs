using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Revive, Setting }

public class GameManager : Singleton<GameManager>
{
    public ColorType WinColorType { get; set; }

    [SerializeField] Transform cameraTrans;

    private static GameState gameState;

    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1920;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void OnInit()
    {
        ChangeState(GameState.MainMenu);
        WinColorType = ColorType.Clear;
        cameraTrans.transform.position = Vector3.zero;
    }
}

