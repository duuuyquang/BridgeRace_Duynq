using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] TextMeshProUGUI cointText;

    public override void Setup()
    {
        base.Setup();
    }

    public void UpdateScore(int coin)
    {
        cointText.text = coin.ToString();
    }

    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
        GameManager.ChangeState(GameState.Setting);
    }
}
