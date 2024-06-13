using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasVictory : UICanvas
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

    public void NextButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>();
        GameManager.Instance.OnInit();
        GameManager.ChangeState(GameState.GamePlay);
        LevelManager.Instance.OnReset();
        LevelManager.Instance.OnLoadLevel(LevelManager.Instance.CurrentLevel+1);
    }
}
