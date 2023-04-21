using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gatto.Utils;

public class EventController : MonoBehaviour
{
    //Lista de eventos de cada tipo
    //-Coisas quebrando
    //-Conflito
    //-Aleatório
    //-Governamental (Fiscal, imposto, essas coisas)
    //-Necessidades (Ausencia de Faxineiro, Segurança, Gerente)
    public bool eventIsOn = true;
    [SerializeField]private float eventTick;
    [SerializeField, Range(.5f,1.5f)]private float tickMultiplier_base;

    private float tick;
    private float tickMultiplier;

    [Space]

    [SerializeField, Range(0f,1f)]private float chanceToBreak_base;
    [SerializeField, Range(0f,1f)]private float chanceToConflict_base;
    [SerializeField, Range(0f,1f)]private float chanceToRandom_base;
    [SerializeField, Range(0f,1f)]private float chanceToGovernment_base;

    private float chanceToBreak;
    private float chanceToConflict;
    private float chanceToRandom;

    private bool hireGuyIsThere = false;
    
    [Space]

    private List<BaseEvent> conflictEvents = new List<BaseEvent>();
    private List<BaseEvent> breakEvents = new List<BaseEvent>();
    private List<BaseEvent> randomEvents = new List<BaseEvent>();

    private List<TemporaryStats> temporaryStats = new List<TemporaryStats>();

    public static EventController i;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        UpdateValue();
        SetupEvents();

        tick = eventTick / tickMultiplier;
    }

    private void Update()
    {
        if(!eventIsOn) return;
        
        if(tick <= 0f)
        {
            tick = eventTick / tickMultiplier;
            TickEvent();
        }else{
            tick -= Time.deltaTime;
        }
    }

    private void SetupEvents()
    {
        #region Conflict Event
        AddEvent(conflictEvents, ()=>
        {
            QueueManager.i.AddQuestionPerson("Rapaziada ta se storano no murro", "Deixa lá", "Manda embora", ()=>{
                AddTemporaryStats(.3f,0f,120f);
            });
        });
        #endregion

        #region Break Event
        AddEvent(breakEvents, ()=>
        {
            QueueManager.i.AddQuestionPerson("Quebrarão o pc do jorgin", "fodac", "compra oto");
            UpdateValue();
        });
        #endregion

        #region Random Event
        AddEvent(randomEvents, ()=>
        {
            if(!HiringManager.i.CanHire()) return;
            if(hireGuyIsThere) return;

            QueueManager.i.AddQuestionPerson("Contrata um cara ai", "Ta", "Nao",()=>{
                HiringManager.i.StartHiring();
                hireGuyIsThere = false;
            }, ()=>{
                hireGuyIsThere = false;
            });

            hireGuyIsThere = true;

            UpdateValue();
        });

        AddEvent(randomEvents, ()=>
        {
            QueueManager.i.AddReportPerson("Arruma 1 e 50 ai parça", "Ok",()=>{
                PaperManager.i.AddContractPaper("Arrumar 1 e 50 para o Paulo Kogos?", ()=>{
                    EarningSystem.i.ChangeMoney(1, "Paulo Kogos");
                });
            });
            UpdateValue();
        });

        #endregion
    }

    private void AddTemporaryStats(float conflict, float breakc, float timer)
    {
        TemporaryStats temp = new TemporaryStats();
        temp.chanceToBreak = breakc;
        temp.chanceToConflict = conflict;

        temporaryStats.Add(temp);

        PeriodTimer.Timer(timer, ()=>{
            temporaryStats.Remove(temp);
            UpdateValue();
        });
        UpdateValue();
    }

    private void AddEvent(List<BaseEvent> listEvent,System.Action addon)
    {
        BaseEvent newEvent = new BaseEvent();
        newEvent.SetEvent(addon);
        listEvent.Add(newEvent);
    }
    private void TickEvent()
    {
        BaseEvent selectedEvent = null;

        int eventTimes = 0;
        while (eventTimes < 2)
        {
            int eventIndex = Random.Range(0, 4);

            float chance = Random.value * 100f;

            if(eventIndex == 0)
            {
                if(chanceToBreak <= chance) selectedEvent = breakEvents[Random.Range(0,breakEvents.Count)];
                
            }else if(eventIndex == 1)
            {
                if(chanceToConflict <= chance) selectedEvent = conflictEvents[Random.Range(0,conflictEvents.Count)];
            }
            else if(eventIndex == 2)
            {
                if(chanceToRandom <= chance) selectedEvent = randomEvents[Random.Range(0,randomEvents.Count)];
            }
            eventTimes++;
        }

        if(selectedEvent != null) selectedEvent.CallEvent();
    }

    public void UpdateValue()
    {
        CurriculumData[] data = CoexistenceManager.i.GetPersons();

        float tickMult = 0f;
        
        float breakChance = 0f;
        float conflictChance = 0f;

        for (int i = 0; i < data.Length; i++)
        {
            Trait[] traits = data[i].GetAllTraits();
            for (int l = 0; l < traits.Length; l++)
            {
                breakChance += traits[l].disastrous;
                conflictChance += traits[l].agressiveness;
                conflictChance += (traits[l].responsability / 2f);

                tickMult += traits[l].agressiveness / 2f;
            }
            
        }

        for (int i = 0; i < temporaryStats.Count; i++)
        {
            conflictChance += temporaryStats[i].chanceToConflict;
            breakChance += temporaryStats[i].chanceToBreak;
        }

        chanceToRandom = ClampValue(chanceToRandom_base);
        tickMultiplier = ClampValue(tickMultiplier_base + tickMult, 0.5f, 1.5f);
        chanceToBreak = ClampValue(chanceToBreak_base + breakChance);
        chanceToConflict = ClampValue(chanceToConflict_base + conflictChance);
    }

    private float ClampValue(float value, float min = 0.1f, float max = 0.75f)
    {
        return Mathf.Clamp(value, min, max);
    }
}

[System.Serializable]
public class BaseEvent
{
    protected System.Action onCall;
    public void CallEvent()
    {
        onCall?.Invoke();
    }

    public void SetEvent(System.Action onCall)
    {
        this.onCall += onCall;
    }
}


public struct TemporaryStats
{
    public float chanceToBreak;
    public float chanceToConflict;
    public float chanceToRandom;
}

