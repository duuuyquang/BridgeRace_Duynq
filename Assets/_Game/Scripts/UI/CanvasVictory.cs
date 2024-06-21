using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        LevelManager.Instance.OnReset();
        int nextLevel = LevelManager.Instance.CurrentLevel + 1;
        if( nextLevel > LevelManager.Instance.LevelMax ) {
            UIManager.Instance.OpenUI<CanvasInformation>();
        } 
        else
        {
            LevelManager.Instance.OnLoadLevel(nextLevel);
            GameManager.Instance.OnInit();
            GameManager.ChangeState(GameState.GamePlay);
            CameraFollower.Instance.SetupGamePlayMode();
            UIManager.Instance.OpenUI<CanvasGamePlay>().UpdateLevelText(LevelManager.Instance.CurrentLevel);
        }
    }
}
