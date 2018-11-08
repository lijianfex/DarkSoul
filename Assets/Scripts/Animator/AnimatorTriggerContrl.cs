using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Animator中的动画事件的ResetTrigger函数
/// </summary>
public class AnimatorTriggerContrl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
}
