using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家输入抽象基类
/// </summary>
public abstract class IPlayerInput : MonoBehaviour {

    [Header("----------Input Enable-------------")]
    public bool InputEnable = true; //是否启用Input

    [Header("-----------角色移动，信号值-------------")]
    public float Dup;   //前后 [-1,1]
    public float Dright;//左右 [-1,1]
    public float Dmag;  //运动值
    public Vector3 Dvec;//运功方向    

    [Header("-----------相机移动，信号值------------")]
    public float JUp;
    public float JRight;


    [Header("-----------按键，信号值------------")]
    //1.pressing signal;
    public bool Run;
    public bool Defense;
    //2.one trigger signal;
    public bool Jump;    
    public bool Attack;
    public bool Roll;
    public bool Lockon;
    //3.double 

   

    //缓动到目标值
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    /// <summary>
    /// 利用论文的6页的公式，将斜向跑动速度快的问题解决 https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}
