using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseState : IState
{
    private float time;
    private float count;

    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Character.ANIM_NAME_FALL);
        enemy.ResetPath();
    }

    public void OnExecute(Enemy enemy)
    {
    }

    public void OnExit(Enemy enemy)
    {
    }
}
