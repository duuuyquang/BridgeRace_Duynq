using System.Collections;
using System.Reflection;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private float lookAtWinnerCamSpd = 5f;
    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GenCharacters(other);
        if (character && GameManager.Instance.Winner == null) {
            GameManager.Instance.Winner = character;
            GameManager.ChangeState(GameState.Finish);
            if (GameManager.Instance.Winner is Enemy)
            {
                GameManager.Instance.Winner.DisableNavMeshAgent();
                GameManager.Instance.Winner.TF.SetParent(transform);
            }
            StartCoroutine(IEWinnerAnimation());
        }
    }

    private IEnumerator IEWinnerAnimation()
    {
        GameManager.Instance.Winner.ChangeAnim(Character.ANIM_NAME_WIN);
        GameManager.Instance.Winner.TF.eulerAngles = new Vector3(0, 180, 0);

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 9f, transform.position.z - 10f);
        //CameraFollower.Instance.TF.position = targetPos;

        float detectWinnerPosSpd = lookAtWinnerCamSpd;
        if(GameManager.Instance.Winner is Enemy)
        {
            detectWinnerPosSpd = 12f;
        }
        while (Vector3.Distance(CameraFollower.Instance.TF.position, targetPos) >= 0.1f)
        {
            CameraFollower.Instance.TF.position = Vector3.MoveTowards(CameraFollower.Instance.TF.position, targetPos, detectWinnerPosSpd * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //CameraFollower.Instance.TF.eulerAngles = new Vector3(30f, 0, 0);

        yield return new WaitForSeconds(.5f);

        targetPos = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
        while (Vector3.Distance(transform.position, targetPos) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, lookAtWinnerCamSpd * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (GameManager.Instance.Winner is Player)
        {
            UIManager.Instance.OpenUI<CanvasVictory>();
        }
        else
        {
            UIManager.Instance.OpenUI<CanvasFail>();
        }
    }
}
