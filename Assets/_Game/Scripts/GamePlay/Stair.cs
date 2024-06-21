using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] Renderer stairRender;
    [SerializeField] protected ColorDataSO colorDataSO;
    public ColorType ColorType { get; set; }



    private void Awake()
    {
        OnInit();
    }

    public void OnInit()
    {
        ChangeColorByType(ColorType.Clear);
    }

    public void ChangeColorByType(ColorType type)
    {
        ColorType = type;
        //stairRender.material.color = DataByType.Colors[(int)type];
        stairRender.material = colorDataSO.GetMat(type);
    }
}
