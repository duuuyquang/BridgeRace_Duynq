using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private const float START_MOVING_DISTANCE_UNIT = 50f;
    
    private Vector3 mouseStartPoint;
    private Vector3 mouseEndPoint;
    private Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
    private Vector3 curDir = Vector3.zero;
    public Vector3 CurDir { get { return curDir; } set { curDir = value; } }

    public void SetCurDirection()
    {
        if (!screenRect.Contains(Input.mousePosition) || Input.GetMouseButtonUp(0))
        {
            curDir = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(0))
        {
            curDir = Vector3.zero;
            mouseStartPoint = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            mouseEndPoint = Input.mousePosition;
            if (Vector3.Distance(mouseStartPoint, mouseEndPoint) >= START_MOVING_DISTANCE_UNIT)
            {
                curDir = mouseEndPoint - mouseStartPoint;
                curDir = new Vector3(curDir.x, 0, curDir.y).normalized;
            }
        }
    }
}
