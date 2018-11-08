using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手柄控制
/// </summary>
public class JoystickInput : IPlayerInput
{
    [Header("--------左摇杆----------------")]
    public string axisX="axisX";
    public string axisY = "axisY";

    [Header("--------右摇杆----------------")]
    public string axisJRight= "axis3";
    public string axisJUp = "axis5";

    [Header("--------手柄按键----------------")]
    public string btnA= "btn0";
    public string btnB= "btn1";
    public string btnC= "btn2";
    public string btnD = "btn3";

    public string btnLB = "btn4";
    public string btnLT = "btn6";

    public string btnJstick = "btn11";

    private ButtonInput ButtonA = new ButtonInput();
    private ButtonInput ButtonB = new ButtonInput();
    private ButtonInput ButtonC = new ButtonInput();
    private ButtonInput ButtonD= new ButtonInput();
    private ButtonInput ButtonLB = new ButtonInput();
    private ButtonInput ButtonLT = new ButtonInput();

    private ButtonInput ButtonJS = new ButtonInput();


    void Update ()
    {
        CheckPlayerMoveInput(InputEnable);//玩家移动

        CheckCameraInput();//相机移动        

        CheckButtonInput();//按键检测


    }

    //检测玩家平面运动输入
    private void CheckPlayerMoveInput(bool isInputEnable)
    {
        if (isInputEnable)
        {
            targetDup = Input.GetAxis(axisY);
            targetDright = Input.GetAxis(axisX);
        }
        else
        {
            targetDup = 0;
            targetDup = 0;
        }
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);          //缓动到目标值
        Dright = Mathf.SmoothDamp(Dright, targetDup, ref velocityDright, 0.1f); //缓动到目标值

        Vector2 tempDAixs = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAixs.x;
        float Dup2 = tempDAixs.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2); //运动量

        Dvec = Dright2 * transform.right + Dup2 * transform.forward; //运动方向
    }

    //检测玩家相机控制输入
    private void CheckCameraInput()
    {
        JUp = Input.GetAxis(axisJUp);
        JRight = Input.GetAxis(axisJRight);
    }

    //按键检测
    private void CheckButtonInput()
    {
        ButtonA.Trick(Input.GetButton(btnA));
        ButtonB.Trick(Input.GetButton(btnB));
        ButtonC.Trick(Input.GetButton(btnC));
        ButtonD.Trick(Input.GetButton(btnD));

        ButtonLB.Trick(Input.GetButton(btnLB));
        ButtonLT.Trick(Input.GetButton(btnLT));
                             
        ButtonJS.Trick(Input.GetButton(btnJstick));

        //A键
        Run = (ButtonA.IsPressing && !ButtonA.IsDelaying) || ButtonA.IsExtending; //长按A键，跑步
        Jump = ButtonA.OnPressed && ButtonA.IsExtending; //双击A键，跳
        Roll= ButtonA.OnRelesed && ButtonA.IsDelaying; //单击A键，翻滚/后撤

        //LB,LT 键
        Attack = ButtonLB.OnPressed;
        Defense = ButtonLT.IsPressing;

        //JS键
        Lockon = ButtonJS.OnPressed;

    }
   
}
