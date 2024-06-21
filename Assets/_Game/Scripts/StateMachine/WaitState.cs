using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : IState
{
    private float time;
    private float count;

    public void OnEnter(Enemy enemy)
    {
        time = Random.Range(1f, 3f);
        count = 0;
        enemy.ChangeAnim(Character.ANIM_NAME_IDLE);
        enemy.ResetPath();
    }

    public void OnExecute(Enemy enemy)
    {
        count += Time.deltaTime;
        if (count >= time) {
            enemy.ChangeState(new SeekBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
