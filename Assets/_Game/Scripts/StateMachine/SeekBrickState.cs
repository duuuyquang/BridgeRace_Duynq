using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBrickState : IState
{
    private int brickNum;

    public void OnEnter(Enemy enemy)
    {
        brickNum = Random.Range(3, enemy.TotalBricksCanCollect + 1);
        enemy.SetNextBrickTarget();
        enemy.SeekBrick();
        enemy.ChangeAnim(Character.ANIM_NAME_RUN);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.CurBrick >= brickNum )
        {
            enemy.ChangeState(new MoveToFinishState());
        }
        else if (enemy.IsDestination) 
        {
            if(Random.Range(0f,10f) > 2f)
            {
                enemy.SeekBrick();
            } else
            {
                enemy.ChangeState(new WaitState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
