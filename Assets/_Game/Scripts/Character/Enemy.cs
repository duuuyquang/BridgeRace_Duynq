using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Enemy : Character
{
    private Vector3 curTargetPos;
    private IState curState;
    private Transform finishPoint;

    [SerializeField] private NavMeshAgent agent;

    private Vector3 destination;

    public bool IsDestination => Vector3.Distance(TF.position, destination + (TF.position.y - destination.y) * Vector3.up) < 0.1f;

    public int TotalBricksCanCollect => stage.TotalBrickEachType;

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        if(agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
    }

    public void ResetPath()
    {
        if(agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    private void Awake()
    {
        OnInit();
    }

    void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {
            if (curState != null)
            {
                curState.OnExecute(this);
            }
        }

        if (GameManager.IsState(GameState.Finish) || GameManager.IsState(GameState.Setting))
        {
            StopMoving();
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        curState = null;
        finishPoint = null;
        curTargetPos = TF.position;
    }

    public void SetNextBrickTarget()
    {
        //curTargetPos = allBricksPos[new Random().Next(allBricksPos.Count)];
        curTargetPos = allBricksPos[UnityEngine.Random.Range(0, allBricksPos.Count-1)];
    }

    public void SeekBrick()
    {
        //TODO: fix agent - done
        SetDestination(curTargetPos);
        if(IsDestination)
        {
            SetNextBrickTarget();
        }

        //agent.velocity = (curTargetPos - TF.position).normalized * speed * Time.deltaTime;
        //if (Vector3.Distance(TF.position, curTargetPos) <= 0.1f)
        //{
        //  SetNextBrickTarget();
        //  if ( UnityEngine.Random.Range(0f, 10f) < 7f ) {
        //      ChangeState(new IdleState());
        //  }
        //  else
        //  {
        //      SetNextBrickTarget();
        //  }
        //}
    }

    public void MovetoFinishPoint()
    {
        if(finishPoint == null)
        {
            //TODO: fix - done
            finishPoint = LevelManager.Instance.CurrentFinishPoint;
        }

        StopMoving();
        SetDestination(finishPoint.position);
    }

    public void StopMoving()
    {
        if(agent)
        {
            //SetDestination(TF.position);
            agent.velocity = Vector3.zero;
        }
    }

    public override void DisableNavMeshAgent()
    {
        agent.enabled = false;
    }

    public void ChangeState(IState newState)
    {   
        if (curState != null)
        {
            curState.OnExit(this);
        }

        curState = newState;

        if (curState != null)
        {
            curState.OnEnter(this);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (TagManager.Compare(other.tag, TagManager.STAGE))
        {
            ChangeState(new IdleState());
        }

        Character character = Cache.GenCharacters(other);
        if (character && character.CurBrick > CurBrick && CurBrick > 0)
        {
            StartCoroutine(IEOnCollapsed());
        }
    }

    private IEnumerator IEOnCollapsed()
    {
        isCollapsed = true;
        ChangeColor(ColorType.Grey);
        ChangeState(new CollapseState());
        while (brickStacks.Count > 0)
        {
            BrickFallAndRemove();
        }

        yield return new WaitForSeconds(2f);

        while (collectedPos.Count > 0)
        {
            RespawnLastCollectedBrick();
        }
        collectedPos.Clear();

        isCollapsed = false;
        fallingSpeed = FALLING_SPEED;
        ChangeColor(ColorType);
        ChangeState(new SeekBrickState());
    }
}
