using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private const float UNIT_BETWEEN_EACH_BRICK = 0.22f;

    [SerializeField] private CharacterBrick characterBrickPrefab;
    [SerializeField] private Transform brickHolder;
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Animator anim;
    [SerializeField] private ColorType colorType;
    [SerializeField] Vector3 startPoint;

    protected List<CharacterBrick> brickStacks = new List<CharacterBrick>();
    private string curAnimName;

    //[SerializeField] protected ColorDataSO colorDataSO;
    protected List<Vector3> allBricksPos = new List<Vector3>();
    protected List<Vector3> collectedPos = new List<Vector3>();
    protected Stage stage;

    public ColorType ColorType { get { return colorType; } set { colorType = value; } }
    public float speed;
    public int CurBrick => brickStacks.Count;

    public virtual void OnInit()
    {
        ChangeColor(ColorType);
        stage = null;
    }

    public virtual void OnDespawn()
    {  
        Destroy(transform.gameObject);
    }

    private void ChangeColor(ColorType type)
    {
        characterRenderer.material.color = DataByType.Colors[(int)type];
    }

    protected void ChangeAnim(string animName)
    {
        if (curAnimName != animName)
        {
            anim.ResetTrigger(animName);
            curAnimName = animName;
            anim.SetTrigger(curAnimName);
        }
    }

    public void AddBrick()
    {
        //CharacterBrick prefab = Instantiate(characterBrickPrefab, Vector3.zero, Quaternion.identity);
        CharacterBrick prefab = SimplePool.Spawn<CharacterBrick>(PoolType.CharacterBrick, Vector3.zero, Quaternion.identity);
        prefab.OnInit(ColorType);
        prefab.transform.SetParent(brickHolder, false);
        prefab.transform.SetLocalPositionAndRotation(new Vector3(0, brickStacks.Count * UNIT_BETWEEN_EACH_BRICK, 0), Quaternion.identity);
        brickStacks.Add(prefab);
    }

    public void RemoveBrick()
    {
        CharacterBrick prefab = brickStacks[CurBrick - 1];
        brickStacks.Remove(prefab);
        //Destroy(prefab.gameObject);
        prefab.OnDespawn();
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

    private void RespawnLastCollectedBrick()
    {
        stage.SpawnEachBrickByType(ColorType, collectedPos[collectedPos.Count - 1]);
        collectedPos.Remove(collectedPos[collectedPos.Count - 1]);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (TagManager.Compare(other.tag, TagManager.BRICK))
        {
            if (other.gameObject.GetComponent<Brick>().CheckValidType(ColorType))
            {
                collectedPos.Add(other.gameObject.transform.position);
                AddBrick();
            }
        }

        if (TagManager.Compare(other.tag, TagManager.STAIR))
        {
            Stair stair = other.gameObject.GetComponent<Stair>();
            if (stair.ColorType != ColorType && CurBrick > 0)
            {
                stair.ChangeColorByType(ColorType);
                RespawnLastCollectedBrick();
                RemoveBrick();
            }
        }

        if (TagManager.Compare(other.tag, TagManager.STAGE))
        {
            Stage stage = other.gameObject.GetComponent<Stage>();
            if(this.stage != stage)
            {
                this.stage = stage;
                allBricksPos.Clear();
                allBricksPos = stage.SpawnBricksByType(ColorType);
            }
        }
    }
}
