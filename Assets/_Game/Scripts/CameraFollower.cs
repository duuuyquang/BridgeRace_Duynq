using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{

    private const float EULER_X_GAME_MENU = 20f;
    private const float EULER_X_GAME_PLAY = 50f;

    private Vector3 OFFSET_GAME_MENU = new Vector3(0f, 5f, -7f);
    private Vector3 OFFSET_GAME_PLAY = new Vector3(0f, 10f, -9f);

    private Vector3 offset = Vector3.zero;
    [SerializeField] private float speed = 1f;
    
    private Vector3 targetPos;

    public Player player;
    public Transform TF;

    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(TagManager.PLAYER).GetComponent<Player>();
        } 
        else if ( !GameManager.IsState(GameState.Finish))
        {
            targetPos = player.TF.position + offset;
            TF.position = Vector3.MoveTowards(TF.position, targetPos, speed * Time.fixedDeltaTime);
        } 
        else
        {
            TF.LookAt(GameManager.Instance.Winner.TF.position);
        }
    }

    public void OnInit()
    {
        TF.position = Vector3.zero;
        SetupMenuMode();
    }

    public void SetupMenuMode()
    {
        TF.eulerAngles = new Vector3(EULER_X_GAME_MENU, 0, 0);
        offset = OFFSET_GAME_MENU;
    }

    public void SetupGamePlayMode()
    {
        offset = OFFSET_GAME_PLAY;
        StartCoroutine(IERotate());
    }

    private IEnumerator IERotate()
    {
        float rotateXDiffer = EULER_X_GAME_PLAY - EULER_X_GAME_MENU;
        float count = 0;
        
        yield return new WaitForSeconds(.5f);

        while ( count++ < rotateXDiffer )
        {
            TF.eulerAngles += Vector3.right;
            yield return new WaitForEndOfFrame();
        }
    }
}
