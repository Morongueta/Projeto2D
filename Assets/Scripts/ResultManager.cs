using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private EarningSystem money;

    private int dayCount;


    private void Start()
    {
        money = EarningSystem.i;

        TimeManager.i.onTimeTick += ()=>{dayCount++;};
    }

    private void Update()
    {
        if(money.GetMoney() >= 20000)
        {
            if(dayCount >= 30)
            {
                //Win
            }
        }else if(money.GetMoney() < 0)
        {
            if(dayCount >= 30)
            {
                //Lose
            }
        }else{
            dayCount = 0;
        }
    }
}
