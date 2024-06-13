using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] GameObject[] buttons;

    public void SetState(UICanvas canvas)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        if (canvas is CanvasMainMenu)
        {
            buttons[2].gameObject.SetActive(true);
        }
        else if (canvas is CanvasGamePlay)
        {
            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        GameManager.Instance.OnInit();
        LevelManager.Instance.OnReset();
        LevelManager.Instance.OnLoadLevel(1);
    }

    public override void Close(float time)
    {
        base.Close(time);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    public void ContinueButton()
    {
        base.CloseDirectly();
        GameManager.ChangeState(GameState.GamePlay);
    }
}
