using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasFail : UICanvas
{
    [SerializeField] TextMeshProUGUI scoreText;

    public void SetBestScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void MainMenuButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        GameManager.Instance.OnInit();
        LevelManager.Instance.OnReset();
        LevelManager.Instance.OnLoadLevel(1);
    }

    public void RetryButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>().UpdateLevelText(LevelManager.Instance.CurrentLevel);
        GameManager.Instance.OnInit();
        LevelManager.Instance.OnReset();
        LevelManager.Instance.OnLoadLevel(LevelManager.Instance.CurrentLevel);
        GameManager.ChangeState(GameState.GamePlay);
        CameraFollower.Instance.SetupGamePlayMode();
    }
}
