using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThreadTesting : MonoBehaviour
{
    private int number = 4;
    private int counter = 0;

    private bool jobRunning;
    private Thread backgroundThread;
    
    private void Start()
    {
        backgroundThread = new Thread(new ThreadStart(DoTheJob));
        backgroundThread.Start();
    }

    private void Update()
    {
        if (counter < number)
        {
            counter++;
            Debug.Log("update says" + counter);
        }
        else
        {
            backgroundThread.Abort();
        }
    }

    void DoTheJob()
    {
        Debug.Log("Thread Started");
        Thread.Sleep(1000);
        Debug.Log("一秒が経過");
        Thread.Sleep(1000);
        Debug.Log("二秒が経過");
        Thread.Sleep(2500);
        counter = 0;
        Debug.Log("I'm done.... time for nap");
    }
}
