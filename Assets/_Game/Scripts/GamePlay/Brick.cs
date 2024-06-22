using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] Renderer brickRender;

    private ColorType type = ColorType.Grey;
    private float height;
    private Character collector;

    public float speed;
    public ColorType Type { get { return type; } set { type = value; } }
    private bool IsDestination => Vector3.Distance(collector.GetCurBrickHolderTopPos(height), TF.position) < 0.3f;

    [SerializeField] protected ColorDataSO colorDataSO;

    private void Update()
    {
        if(collector)
        {
            MovingToCollectorHolder();
        }
    }

    public void OnInit(ColorType type)
    {
        this.type = type;
        height = brickRender.gameObject.transform.lossyScale.y;
        ChangeColor();
    }

    public void ChangeColor()
    {
        //brickRender.material.color = DataByType.Colors[(int)type];
        brickRender.material = colorDataSO.GetMat(type);
    }

    public void OnDespawn()
    {
        collector = null;
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isCharacter = TagManager.Compare(other.tag, TagManager.PLAYER) || TagManager.Compare(other.tag, TagManager.ENEMY);
        //TODO: CACHE - done
        Character character = Cache.GenCharacters(other);
        if (isCharacter && CheckValidType(character.ColorType) && character.IsAvailableToCollect())
        {
            if(type != ColorType.Grey)
            {
                character.CacheCollectedPos(TF.position);
            }
            collector = character;
            ////Destroy(transform.gameObject);
            //OnDespawn();
        }
    }

    private void MovingToCollectorHolder()
    {
        if (!IsDestination)
        {
            TF.position = Vector3.MoveTowards(TF.position, collector.GetCurBrickHolderTopPos(height), speed * Time.deltaTime);
        } 
        else
        {
            if(collector.IsAvailableToCollect())
            {
                collector.AddBrick();
            }
            else if(type != ColorType.Grey)
            {
                collector.RespawnLastCollectedBrick();
            }
            OnDespawn();
        }
    }

    public bool CheckValidType(ColorType type)
    {
        return type == this.type || this.type == ColorType.Grey;
    }
}
