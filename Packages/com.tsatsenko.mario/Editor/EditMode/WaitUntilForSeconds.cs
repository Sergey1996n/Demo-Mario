using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUntilForSeconds : CustomYieldInstruction
{
    protected float maxTime;
    protected float timer = 0;
    protected string nameMethod;
    protected Func<bool> myChecker;
    public WaitUntilForSeconds(Func<bool> myChecker, float maxTime, string nameMethod)
    {
        this.myChecker = myChecker;
        this.maxTime = maxTime;
        this.nameMethod = nameMethod;

        //waitingForFirst = true;
    }
    public override bool keepWaiting
    {
        get
        {
            bool checkThisTurn = myChecker();
            if (checkThisTurn)
            {
                return true;
            }
            timer += Time.deltaTime;
            if (maxTime < timer)
            {
                Assert.Fail("The \"{0}\" test execution time is more than 10 seconds", new object[] { nameMethod });
                return true;
            }
            return true;
        }
    }
}
