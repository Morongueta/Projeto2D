using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    private EarningSystem money;

    private int dayCount;

    [Header("UI")]
    [SerializeField]private GameObject resultObject;
    [SerializeField]private TextMeshProUGUI resultText;

    private float timeToFinish = 5f;

    private bool gameFinished;
    private bool transitionStarted;

    private void Start()
    {
        money = EarningSystem.i;
        timeToFinish = 5f;
        TimeManager.i.onTimeTick += ()=>{dayCount++;};
    }

    private void Update()
    {
        if(gameFinished)
        {
            resultObject.SetActive(true);
            QueueManager.i.RemoveFromQueue(0);
            if(timeToFinish <= 0f)
            {
                if(!transitionStarted)TransitionManager.i.PlayScene(TransitionStyle.FADE, TransitionState.IN, TransitionState.OUT, "Menu");
                transitionStarted = true;
            }else{
                timeToFinish -= Time.deltaTime;
            }
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.B))
        {
            resultText.text = "Prosperou!";
            gameFinished = true;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            resultText.text = "Faliu!";
            gameFinished = true;
        }

        if(money.GetMoney() >= 20000)
        {
            if(dayCount >= 30)
            {
                //Win
                resultText.text = "Prosperou!";
                gameFinished = true;
            }
        }else if(money.GetMoney() < 0)
        {
            if(dayCount >= 30)
            {
                //Lose
                resultText.text = "Faliu!";
                gameFinished = true;
            }
        }else{
            dayCount = 0;
        }
    }
}
