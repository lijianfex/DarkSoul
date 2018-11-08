using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按键输入类
/// </summary>
public class ButtonInput
{
    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnRelesed = false;
    public bool IsExtending = false;
    public bool IsDelaying = false;

    private bool curState=false;
    private bool lastState=false;

    public float extendingDuration = 0.3f; //延长时间
    public float delayingDuration = 0.3f;   //延迟时间

    private Timer extTimer = new Timer();
    private Timer delayTimer = new Timer();

    public void Trick(bool input)
    {
        extTimer.UpdataTimer();
        delayTimer.UpdataTimer();

        curState = input;
        IsPressing = curState;

        OnPressed = false;
        OnRelesed = false;
        IsExtending = false;
        IsDelaying = false;
        if (curState!=lastState)
        {
            if(curState==true)
            {
                OnPressed = true;
                StartTimer(delayTimer, delayingDuration);
            }
            else
            {
                OnRelesed = true;
                StartTimer(extTimer, extendingDuration);
            }
        }
        lastState = curState;

        if(extTimer.state==Timer.STATE.RUN)
        {
            IsExtending = true;
        }
        
        if(delayTimer.state==Timer.STATE.RUN)
        {
            IsDelaying = true;
        }
    }

    private void StartTimer(Timer timer,float duration)
    {
        timer.DurationTime = duration;
        timer.EnableTimer();
    }

}
