using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorAvt;

    private void OnTriggerEnter(Collider collider) 
    {
        if (TagManager.Compare(collider.tag, TagManager.PLAYER) && collider.gameObject.GetComponent<Player>().IsMovingBack)
        {
            return;
        }
        StartCoroutine(IEOpen());
    }

    private IEnumerator IEOpen()
    {
        while (doorAvt.localPosition.y > -4f)
        {
            doorAvt.localPosition += Vector3.down * 0.1f;
            yield return new WaitForEndOfFrame();
        }
    }
}
