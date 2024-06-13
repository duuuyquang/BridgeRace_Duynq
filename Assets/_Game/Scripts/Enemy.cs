using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Enemy : Character
{
    private Vector3 curTargetPos;
    private IState curState;
    private NavMeshAgent agent;
    private Transform finishPoint;

    private void Awake()
    {
        OnInit();
        agent = GetComponent<NavMeshAgent>();
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
        curTargetPos = transform.position;
    }

    public void SetNextBrickTarget()
    {
        curTargetPos = allBricksPos[new Random().Next(allBricksPos.Count)];
    }

    public void SeekBrick()
    {
        agent.velocity = (curTargetPos - transform.position).normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, curTargetPos) <= 0.1f)
        {
            SetNextBrickTarget();
        }
    }

    public void MovetoFinishPoint()
    {
        if(finishPoint == null)
        {
            finishPoint = GameObject.FindGameObjectWithTag(TagManager.FINISH).transform;
        }

        StopMoving();
        agent.SetDestination(finishPoint.position);
    }

    public void StopMoving()
    {
        if(agent)
        {
            agent.velocity = Vector3.zero;
        }
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
        if (TagManager.Compare(other.tag, TagManager.FINISH) && GameManager.Instance.WinColorType == ColorType.Clear)
        {
            GameManager.ChangeState(GameState.Finish);
            GameManager.Instance.WinColorType = ColorType;
            UIManager.Instance.OpenUI<CanvasFail>();
        }
    }
}
