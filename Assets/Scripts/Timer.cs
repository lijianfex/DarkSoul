using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public enum STATE
    {
        IDLE,
        RUN,
        END
    }

    public STATE state;

    public float DurationTime = 1.0f;//持续时间

    public float ElapsedTime = 0.0f;//已经过去的时间

    public void UpdataTimer()
    {
        switch (state)
        {
            case STATE.IDLE:

                break;
            case STATE.RUN:
                ElapsedTime += Time.deltaTime;
                if(ElapsedTime>= DurationTime)
                {
                    state = STATE.END;
                }
                break;
            case STATE.END:
                break;
            default:
                break;
        }
    }

    public void EnableTimer()
    {
        ElapsedTime = 0.0f;
        state = STATE.RUN;
    }
}