using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform targetObjTransform;

    [SerializeField] private Vector3 offset = new Vector3(0f, 11f, -7f);
    [SerializeField] private float speed = 1f;

    private Vector3 targetPos;

    void LateUpdate()
    {
        if (targetObjTransform == null)
        {
            targetObjTransform = GameObject.FindGameObjectWithTag(TagManager.PLAYER).transform;
        } 
        else
        {
            targetPos = targetObjTransform.position + offset;
            //transform.position = Vector3.Lerp(transform.position, targetPos, 2f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
        }
    }

    public void OnInit()
    {
        targetPos = targetObjTransform.transform.position + offset;
        transform.position = targetPos;
    }
}
