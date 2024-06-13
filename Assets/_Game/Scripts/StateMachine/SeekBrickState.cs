using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBrickState : IState
{
    private int brickNum;

    public void OnEnter(Enemy enemy)
    {
        brickNum = Random.Range(3, 11);
        enemy.SetNextBrickTarget();
    }

    public void OnExecute(Enemy enemy)
    {
        enemy.SeekBrick();
        if (enemy.CurBrick >= brickNum)
        {
            enemy.ChangeState(new MoveToFinishState());
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
