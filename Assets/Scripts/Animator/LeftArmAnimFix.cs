using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 左手臂，Ik调整
/// </summary>
public class LeftArmAnimFix : MonoBehaviour {

    private Animator anim;

    public Vector3 LeftArm;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if(anim.GetBool("defense")==false)
        {
            Transform LeftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            LeftLowerArm.localEulerAngles += LeftArm;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(LeftLowerArm.localEulerAngles));
        }        
        
    }
}
