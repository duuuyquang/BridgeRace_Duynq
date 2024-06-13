using UnityEngine;

public class CharacterBrick : GameUnit
{
    [SerializeField] Renderer brickRenderer;

    public void OnInit(ColorType type)
    {
        brickRenderer.material.color = DataByType.Colors[(int)type];
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
