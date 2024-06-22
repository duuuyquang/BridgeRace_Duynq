using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] Rigidbody rb;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    [SerializeField] Transform rayCastPos;
    [SerializeField] LayerMask layerMask;
    private RaycastHit slopeHit;
    private float slopeAngle;

    private RaycastHit frontHit;
    private Vector3 fallBackDir => (PlayerController.Instance.CurDir - TF.position).normalized;

    public override bool IsMovingBack => PlayerController.Instance.CurDir.z < 0;

    private void Start()
    {
        OnInit();
    }

    void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {
            ListenControllerInput();
        }
    }

    void FixedUpdate()
    {   
        if (GameManager.IsState(GameState.Finish))
        {
            StopMoving();
        }

        if (GameManager.IsState(GameState.GamePlay))
        {
            ProcessMoving();
            RayCastCheckFront();
        }
    }

    public override void OnDespawn()
    {
        OnReset();
    }

    private void ProcessMoving()
    {
        if (isCollapsed)
        {
            rb.velocity = -PlayerController.Instance.CurDir * (fallingSpeed--) * Time.fixedDeltaTime;
            ChangeAnim(ANIM_NAME_FALL);
            return;
        }

        Vector3 finalDir = PlayerController.Instance.CurDir;
        if(Vector3.Distance(finalDir, Vector3.zero) <= 0.1f)
        {
            ChangeAnim(ANIM_NAME_IDLE);
        } else
        {
            ChangeAnim(ANIM_NAME_RUN);
        }

        if (OnSlope() && Input.GetMouseButton(0))
        {
            finalDir = GetSlopeMoveDirection();
        }

        rb.velocity = finalDir * speed * Time.fixedDeltaTime;
    }

    private void ListenControllerInput()
    {
        if(isCollapsed)
        {
            return;
        }
        PlayerController.Instance.SetCurDirection();
        LookAtCurDirection();
    }

    private void LookAtCurDirection()
    {
        if (Vector3.Distance(rb.velocity, Vector3.zero) >= 0.1f)
        {
            TF.LookAt(TF.position + PlayerController.Instance.CurDir);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(rayCastPos.position, Vector3.down, out slopeHit, 1f, layerMask))
        {
            slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(PlayerController.Instance.CurDir, slopeHit.normal).normalized;
    }

    private void RayCastCheckFront()
    {
        //Debug.DrawLine(TF.position, TF.position + PlayerController.Instance.CurDir, UnityEngine.Color.red);
        if (Physics.Raycast(TF.position, PlayerController.Instance.CurDir, out frontHit, .5f))
        {
            if(TagManager.Compare(frontHit.collider.tag, TagManager.STAIR))
            {
                Stair stair = frontHit.collider.gameObject.GetComponent<Stair>();
                if (CurBrick <= 0 && stair.ColorType!= ColorType && !IsMovingBack)
                {
                    rb.velocity = Vector3.zero;
                }
            }

            if (TagManager.Compare(frontHit.collider.tag, TagManager.DOOR))
            {
                if (IsMovingBack)
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    private void OnReset()
    {
        PlayerController.Instance.CurDir = Vector3.zero;
        TF.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        RemoveAllBrick();
        collectedPos.Clear();
        allBricksPos.Clear();
        ChangeAnim(ANIM_NAME_IDLE);
    }

    public override bool IsAvailableToCollect()
    {
        return !isCollapsed;
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (TagManager.Compare(other.tag, TagManager.FINISH))
        {
            StopMoving();
        }

        Character character = Cache.GenCharacters(other);
        if (character && character.CurBrick >= CurBrick && CurBrick > 0)
        {
            StartCoroutine(IEOnCollapsed());
        }
    }

    private IEnumerator IEOnCollapsed()
    {
        isCollapsed = true;
        ChangeColor(ColorType.Grey);

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
    }
}