using UnityEngine;

public class CharacterBrick : GameUnit
{
    [SerializeField] Renderer brickRenderer;
    [SerializeField] protected ColorDataSO colorDataSO;

    private bool isFalling = false;
    private Vector3 targetFallingPos;

    private bool IsDestination => Vector3.Distance(TF.position, targetFallingPos) < 0.1f;

    public void Update()
    {
        if(isFalling)
        {
            ProccessFalling();
        }
    }

    public void ProccessFalling()
    {
        TF.position = Vector3.MoveTowards(TF.position, targetFallingPos, 6f * Time.deltaTime);
        if(IsDestination)
        {
            isFalling = false;
            OnDespawn();
        }
    }

    public void SetFalling()
    {
        targetFallingPos = new Vector3(TF.position.x, -1, TF.position.z) + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4));
        ChangeColor(ColorType.Grey);
        isFalling = true;
    }

    public void ChangeColor(ColorType type)
    {
        brickRenderer.material = colorDataSO.GetMat(type);
    }

    public void OnInit(ColorType type)
    {
        //brickRenderer.material.color = DataByType.Colors[(int)type];
        brickRenderer.material = colorDataSO.GetMat(type);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
