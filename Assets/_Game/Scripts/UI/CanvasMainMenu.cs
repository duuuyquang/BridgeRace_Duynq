using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>().UpdateLevelText(LevelManager.Instance.CurrentLevel);
        GameManager.ChangeState(GameState.GamePlay);
        CameraFollower.Instance.SetupGamePlayMode();
    }

    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
        GameManager.ChangeState(GameState.Setting);
    }
}
