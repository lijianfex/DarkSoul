using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来控制模型自带位移，应用到角色移动上
/// </summary>
public class RootMationContrl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMation",(object)anim.deltaPosition);
    }
}
