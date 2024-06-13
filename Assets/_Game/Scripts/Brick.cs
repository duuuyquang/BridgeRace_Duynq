using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] Renderer brickRender;
    private ColorType type = ColorType.Grey;
    public ColorType Type { get { return type; } set { type = value; } }

    public void OnInit(ColorType type)
    {
        this.type = type;
        brickRender.material.color = DataByType.Colors[(int)type];
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isCharacter = TagManager.Compare(other.tag, TagManager.PLAYER) || TagManager.Compare(other.tag, TagManager.ENEMY);

        if (isCharacter && CheckValidType(other.GetComponent<Character>().ColorType))
        {
            //Destroy(transform.gameObject);
            OnDespawn();
        }
    }

    public bool CheckValidType(ColorType type)
    {
        return type == this.type || this.type == ColorType.Grey;
    }
}
