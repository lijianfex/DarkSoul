using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家输入模块
/// </summary>
public class KeyBoardInput : IPlayerInput
{
    

    [Header("--------移动控制键----------------")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";

    

    [Header("-----------相机控制键-----------------------")]
    public string KeyJUp="up";
    public string KeyJDown="down";
    public string KeyJRight="right";
    public string KeyJLeft="left";
    
    [Header("-----------鼠标控制-----------------------")]
    public bool MouseEnable;
    public float MouseXsensity=1.0f;
    public float MouseYsensity=1.0f;


    [Header("-----------跑跳，技能键------------------")]
    public string KeyA = "left shift";//加速跑
    public string KeyB = "space";//跳跃
    public string KeyC = "mouse 0";//攻击
    public string KeyD = "mouse 1"; //举盾

    public string KeyLockon = "x";

    private ButtonInput btnA = new ButtonInput();
    private ButtonInput btnB= new ButtonInput();
    private ButtonInput btnC = new ButtonInput();
    private ButtonInput btnD = new ButtonInput();

    private ButtonInput btnLockon = new ButtonInput();



    void Update()
    {
        
        CheckPlayerMoveInput(InputEnable);//玩家移动

        CheckCameraInput(MouseEnable);//相机移动

        CheckButtonInput();//按键输入    
        
        
       
    }

    //检测玩家平面运动输入
    private void CheckPlayerMoveInput(bool isInputEnable)
    {
        if (isInputEnable)
        {
            targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
            targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);           
        }
        else
        {
            targetDup = 0;
            targetDright = 0;
        }
        
        //得到移动量与移动方向
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);          //缓动到目标值
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f); //缓动到目标值

        Vector2 tempDAixs = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAixs.x;
        float Dup2 = tempDAixs.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2); //运动量

        Dvec = Dright2 * transform.right + Dup2 * transform.forward; //运动方向

    }

    //检测玩家相机控制输入
    private void CheckCameraInput(bool mouseEnable)
    {
        if(mouseEnable)
        {
            JUp = Input.GetAxis("Mouse Y")* MouseYsensity;
            JRight = Input.GetAxis("Mouse X")* MouseXsensity;
        }
        else
        {
            JUp = (Input.GetKey(KeyJUp) ? 1.0f : 0) - (Input.GetKey(KeyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(KeyJRight) ? 1.0f : 0) - (Input.GetKey(KeyJLeft) ? 1.0f : 0);
        }
        

    }

    
    //按键检测
    private void CheckButtonInput()
    {
        btnA.Trick(Input.GetKey(KeyA));
        btnB.Trick(Input.GetKey(KeyB));
        btnC.Trick(Input.GetKey(KeyC));
        btnD.Trick(Input.GetKey(KeyD));

        btnLockon.Trick(Input.GetKey(KeyLockon));
       
        //A键
        Run = (btnA.IsPressing && !btnA.IsDelaying)|| btnA.IsExtending;//长按shift键，跑步

        Jump = btnA.OnPressed && btnA.IsExtending;//双击A键，跳

        Roll = btnA.OnRelesed && btnA.IsDelaying;//单击A键，翻滚/后撤

        //Run=btnA.IsPressing;
        //Jump = btnB.OnPressed;
        Attack = btnC.OnPressed;
        Defense = btnD.IsPressing;

        //锁定
        Lockon = btnLockon.OnPressed;
        
        
    }








}