using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 落地检测器
/// </summary>
public class OnGroundSensor : MonoBehaviour
{

    public CapsuleCollider CapsuleCol; //PalyerHandle

    public float offest = 0.2f; //向下偏移量

    private Vector3 point1; //脚部点
    private Vector3 point2; //头部点
    private float radius;  //检测器的半径



    void Awake()
    {
        radius = CapsuleCol.radius-0.05f;

    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius-offest);
        point2 = transform.position + transform.up * (CapsuleCol.height-offest) - transform.up * radius;

        Collider[] outPutCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (outPutCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");            
        }

    }
}
