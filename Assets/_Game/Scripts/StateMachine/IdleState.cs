using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float time;
    private float count = 0;
    public void OnEnter(Enemy enemy)
    {
        time = Random.Range(0f, 3f);
        enemy.ResetPath();
        enemy.ChangeAnim(Character.ANIM_NAME_IDLE);
    }

    public void OnExecute(Enemy enemy)
    {
        count += Time.deltaTime;
        if (count >= time)
        {
            enemy.ChangeState(new SeekBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
