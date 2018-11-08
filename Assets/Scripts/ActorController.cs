using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色控制器
/// </summary>
public class ActorController : MonoBehaviour
{

    public GameObject Model; //角色模型
    public IPlayerInput pi; //玩家输入

    public CameraController cameraCtrl;

    public float WalkSpeed = 2.4f;//行走速度
    public float RunMutiplier = 2.7f; //跑步加倍值

    public float JumpVelocity = 4.0f;//跳跃值
    public float RollVelocity = 3.0f;//滚动值

    private float CurrentSpeed;
    private float velocitySpeed;


    [Space(10)]
    [Header("----------摩擦力设置-------------")]
    public PhysicMaterial FirtionOne;
    public PhysicMaterial FirtionZero;


    private Animator anim; //动画控制器
    private Rigidbody rigid;//刚体
    private Vector3 planeMoveVec; //平面移动量
    private Vector3 thrusVec; //向上的增量
    private CapsuleCollider capsuleCol;//碰撞体

    private float lerpWeightTarget;//layer weight target
    private Vector3 detalPostion;

    private bool canAttack;
    private bool lockPlaneMoveVec = false;
    private bool trackDeraction = false;



    void Awake()
    {
        IPlayerInput[] inputs = GetComponents<IPlayerInput>();
        foreach (IPlayerInput input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }

        anim = Model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            Debug.LogError("model don't have rigdbody compment!");
        }

        capsuleCol = GetComponent<CapsuleCollider>();
        if (capsuleCol == null)
        {
            Debug.LogError("model don't have CapsuleCollider compment!");
        }
    }


    void Update()
    {
        //锁定
        if (pi.Lockon)
        {
            cameraCtrl.LockUnLock();
        }

        if(cameraCtrl.LockState==false)
        {
            //行动动画,缓动到跑动动画
            float targetRunMuti = (pi.Run) ? 2.0f : 1.0f;
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMuti, 0.4f));
            anim.SetFloat("right", 0f);
        }
        else
        {
            Vector3 LocalDvec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", LocalDvec.z * ((pi.Run) ? 2.0f : 1.0f));
            anim.SetFloat("right", LocalDvec.x * ((pi.Run) ? 2.0f : 1.0f));
        }
        

        anim.SetBool("defense", pi.Defense);

        //跳跃动画
        if (pi.Jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

       

        //攻击动画
        if (pi.Attack && CheckState("ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }

        //判断是否能从Fall-->roll 
        if (pi.Roll || rigid.velocity.magnitude > 7.0f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if(cameraCtrl.LockState==false)
        {
            //转向，缓动转向
            if (pi.Dmag > 0.1f)
            {
                Vector3 targetForward = Vector3.Slerp(Model.transform.forward, pi.Dvec, 0.4f);
                Model.transform.forward = targetForward;
            }

            //移动向量，如果run,就将移动向量加倍
            if (lockPlaneMoveVec == false)
            {
                CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, WalkSpeed * ((pi.Run) ? RunMutiplier : 1.0f), ref velocitySpeed, 0.4f);

                planeMoveVec = pi.Dmag * Model.transform.forward * CurrentSpeed;
            }
        }
        else
        {
            if (trackDeraction == false)
            {
                Model.transform.forward = transform.forward;
            }
            else
            {
                Model.transform.forward = planeMoveVec.normalized;
            }
           
            if(lockPlaneMoveVec == false)
            {
                planeMoveVec = pi.Dvec * WalkSpeed * ((pi.Run) ? RunMutiplier : 1.0f);
            }
            
        }

        

    }

    //物理相关
    private void FixedUpdate()
    {
        rigid.position += detalPostion;
        //rigid.position += planeMoveVec * Time.fixedDeltaTime; //直接改位置    
        rigid.velocity = new Vector3(planeMoveVec.x, rigid.velocity.y, planeMoveVec.z) + thrusVec; //改刚体的速度（不能直接将planeMoveVec赋给velocity）
        thrusVec = Vector3.zero;
        detalPostion = Vector3.zero;
    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);

        return anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
    }


    /// 
    /// 处理其他模块的消息 Messges
    /// 

    public void OnJumpEnter()
    {
        pi.InputEnable = false;
        lockPlaneMoveVec = true;
        thrusVec = new Vector3(0f, JumpVelocity, 0f);
        trackDeraction = true;
    }


    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }


    public void OnGroundEnter()
    {
        pi.InputEnable = true;
        lockPlaneMoveVec = false;
        canAttack = true;
        capsuleCol.material = FirtionOne;
        trackDeraction = false;

    }

    public void OnGroundExit()
    {
        capsuleCol.material = FirtionZero;
    }

    public void OnFallEnter()
    {
        pi.InputEnable = false;
        lockPlaneMoveVec = true;
    }

    public void OnRollEnter()
    {
        pi.InputEnable = false;
        lockPlaneMoveVec = true;
        thrusVec = new Vector3(0f, RollVelocity, 0f);
        trackDeraction = true;
    }

    public void OnJabEnter()
    {
        pi.InputEnable = false;
        lockPlaneMoveVec = true;
    }

    public void OnJabUpdate()
    {
        thrusVec = Model.transform.forward * anim.GetFloat("jabVelocity");
    }



    public void OnAttackIdleEnter()
    {
        pi.InputEnable = true;
        lerpWeightTarget = 0.0f;
    }

    public void OnAttackIdleUpdate()
    {
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpWeightTarget, 0.4f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnAttack1hAEnter()
    {
        pi.InputEnable = false;
        lerpWeightTarget = 1.0f;
    }

    public void OnAttack1hUpdate()
    {
        thrusVec = Model.transform.forward * anim.GetFloat("attack1hAVelocity");

        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpWeightTarget, 0.4f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnUpdateRootMation(object _detalPos)
    {
        if (CheckState("attack1hC", "Attack"))
        {
            detalPostion += (0.8f * detalPostion + 0.2f * (Vector3)_detalPos) / 1.0f;
        }
    }
}
