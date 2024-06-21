using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFinishState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.MovetoFinishPoint();
        enemy.ChangeAnim(Character.ANIM_NAME_RUN);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.CurBrick <= 0)
        {
            enemy.StopMoving();
            enemy.ChangeState(new SeekBrickState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
