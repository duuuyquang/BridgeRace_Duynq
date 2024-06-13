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
    private RaycastHit frontHit;

    private float slopeAngle;

    public bool IsMovingBack => PlayerController.Instance.CurDir.z < 0;

    // Update is called once per frame
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
        if(GameManager.IsState(GameState.Finish))
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
        Vector3 finalDir = PlayerController.Instance.CurDir;
        if (OnSlope() && Input.GetMouseButton(0))
        {
            finalDir = GetSlopeMoveDirection();
        }
        rb.velocity = finalDir * speed * Time.fixedDeltaTime;
    }

    private void ListenControllerInput()
    {
        PlayerController.Instance.SetCurDirection();
        LookAtCurDirection();
    }

    private void LookAtCurDirection()
    {
        if (Vector3.Distance(rb.velocity, Vector3.zero) >= 0.1f)
        {
            transform.LookAt(transform.position + PlayerController.Instance.CurDir);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(rayCastPos.position, Vector3.down, out slopeHit, 1f, layerMask))
        {
            slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            Debug.Log(slopeAngle);
            //Debug.Log(slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        Vector3 temp = new Vector3(PlayerController.Instance.CurDir.x, 0f, 1f);
        if (IsMovingBack)
        {
            temp = new Vector3(PlayerController.Instance.CurDir.x, -1f, 0);
        }

        return Vector3.ProjectOnPlane(temp, slopeHit.normal).normalized;
    }

    private void RayCastCheckFront()
    {
        Debug.DrawLine(transform.position, transform.position + PlayerController.Instance.CurDir, UnityEngine.Color.red);
        if (Physics.Raycast(transform.position, PlayerController.Instance.CurDir, out frontHit, .5f))
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
        transform.position = Vector3.zero;
        RemoveAllBrick();
        collectedPos.Clear();
        allBricksPos.Clear();
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (TagManager.Compare(other.tag, TagManager.FINISH) && GameManager.Instance.WinColorType == ColorType.Clear)
        {
            GameManager.ChangeState(GameState.Finish);
            GameManager.Instance.WinColorType = ColorType;
            UIManager.Instance.OpenUI<CanvasVictory>();
            StopMoving();
        }
    }
}