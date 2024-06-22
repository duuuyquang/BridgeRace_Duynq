using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private const float UNIT_BETWEEN_EACH_BRICK = 0.22f;

    public const string ANIM_NAME_WIN    = "win";
    public const string ANIM_NAME_LOSE   = "lose";
    public const string ANIM_NAME_RUN    = "run";
    public const string ANIM_NAME_IDLE   = "idle";
    public const string ANIM_NAME_FALL   = "fall";

    protected const float FALLING_SPEED  = 100f;

    [SerializeField] private CharacterBrick characterBrickPrefab;
    [SerializeField] private Transform brickHolder;
    [SerializeField] protected Renderer characterRenderer;
    [SerializeField] private Animator anim;
    [SerializeField] private ColorType colorType;
    [SerializeField] Vector3 startPoint;
    [SerializeField] protected Transform tf;
    [SerializeField] protected Transform brickHolderTF;

    protected List<CharacterBrick> brickStacks = new List<CharacterBrick>();
    private string curAnimName;

    [SerializeField] protected ColorDataSO colorDataSO;
    protected List<Vector3> allBricksPos = new List<Vector3>();
    protected List<Vector3> collectedPos = new List<Vector3>();
    protected Stage stage;

    protected bool  isCollapsed = false;
    protected float fallingSpeed = FALLING_SPEED;

    public ColorType ColorType { get { return colorType; } set { colorType = value; } }
    public float speed;
    public int CurBrick => brickStacks.Count;
    public virtual bool IsMovingBack => PlayerController.Instance.CurDir.z < 0;
    public Transform TF { get { return tf; } }

    public virtual void OnInit()
    {
        ChangeColor(ColorType);
        stage = null;
    }

    public virtual void OnDespawn()
    {  
        Destroy(transform.gameObject);
    }

    public void ChangeColor(ColorType type)
    {
        //TODO: FIX
        //characterRenderer.material.color = DataByType.Colors[(int)type];
        characterRenderer.material = colorDataSO.GetMat(type);
    }

    public void ChangeAnim(string animName)
    {
        if (curAnimName != animName)
        {
            anim.ResetTrigger(curAnimName);
            curAnimName = animName;
            anim.SetTrigger(curAnimName);
        }
    }

    public virtual bool IsAvailableToCollect()
    {
        return !isCollapsed;
    }

    public void AddBrick()
    {
        //CharacterBrick prefab = Instantiate(characterBrickPrefab, Vector3.zero, Quaternion.identity);
        CharacterBrick brick = SimplePool.Spawn<CharacterBrick>(PoolType.CharacterBrick, Vector3.zero, Quaternion.identity);
        brick.OnInit(ColorType);
        brick.transform.SetParent(brickHolder, false);
        brick.transform.SetLocalPositionAndRotation(new Vector3(0, brickStacks.Count * UNIT_BETWEEN_EACH_BRICK, 0), Quaternion.identity);
        brickStacks.Add(brick);
    }

    public Vector3 GetCurBrickHolderTopPos(float brickHeight)
    {
        return brickHolderTF.position + new Vector3(0, brickStacks.Count * UNIT_BETWEEN_EACH_BRICK + brickHeight/2, 0);
    }

    public void RemoveBrick()
    {
        CharacterBrick brick = brickStacks[CurBrick - 1];
        brickStacks.Remove(brick);
        //Destroy(prefab.gameObject);
        brick.OnDespawn();
    }

    public void BrickFallAndRemove()
    {
        CharacterBrick brick = brickStacks[CurBrick - 1];
        brick.SetFalling(TF);
        brickStacks.Remove(brick);
    }

    public void RemoveAllBrick()
    {
        while(brickStacks.Count > 0)
        {
            RemoveBrick();
        }

        brickStacks.Clear();
        SimplePool.Release(PoolType.CharacterBrick);
    }

    public virtual void DisableNavMeshAgent()
    {

    }
    public void RespawnLastCollectedBrick()
    {
        if(collectedPos.Count > 0)
        {
            stage.SpawnEachBrickByType(ColorType, collectedPos[collectedPos.Count - 1]);
            collectedPos.Remove(collectedPos[collectedPos.Count - 1]);
        }
    }

    public void CacheCollectedPos(Vector3 position)
    {
        collectedPos.Add(position);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //if (TagManager.Compare(other.tag, TagManager.BRICK))
        //{
        //    //TODO: cache - done
        //    //Brick brick = other.gameObject.GetComponent<Brick>();
        //    Brick brick = Cache.GenBricks(other);
        //    if (brick.CheckValidType(ColorType))
        //    {
        //        CacheCollectedPos(other.gameObject.transform.position);
        //        AddBrick();
        //    }
        //}

        if (TagManager.Compare(other.tag, TagManager.STAIR))
        {
            //TODO: cache - done
            //Stair stair = other.gameObject.GetComponent<Stair>();
            Stair stair = Cache.GenStairs(other);
            if (stair.ColorType != ColorType && CurBrick > 0)
            {
                stair.ChangeColorByType(ColorType);
                RespawnLastCollectedBrick();
                RemoveBrick();
            }
        }

        if (TagManager.Compare(other.tag, TagManager.STAGE))
        {
            //TODO: cache - done
            //Stage stage = other.gameObject.GetComponent<Stage>();
            Stage stage = Cache.GenStages(other);
            if (this.stage != stage)
            {
                this.stage = stage;
                allBricksPos.Clear();
                allBricksPos = stage.SpawnBricksByType(ColorType);
            }
        }
    }
}
