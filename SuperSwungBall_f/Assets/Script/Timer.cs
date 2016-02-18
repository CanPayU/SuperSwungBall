using UnityEngine;
using System.Collections;
using System;

public class Timer {


    private bool started;
    private float max_time;
    private float time_remaining;

    private Action end_time;

    public Timer(float max_, Action end)
    {
        max_time = max_;
        time_remaining = max_;
        started = false;
        end_time = end;
    }

    public void start()
    {
        time_remaining = max_time;
        started = true;
    }

    public void update()
    {
        if (time_remaining > 0 && started)
            time_remaining -= Time.deltaTime;
        else if (started)
        {
            started = false;
            end_time();
            time_remaining = 0;
        }
    }

    public float Time_remaining
    {
        get {    return time_remaining; }
    }



}
