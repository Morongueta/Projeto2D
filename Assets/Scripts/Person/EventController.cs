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

    private float tempIncrease;

    public bool eventIsOn = true;
    [SerializeField]private Curriculum bossCurriculum;
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
            if(CoexistenceManager.i.GetPersons().Length < 3) return;
            CurriculumData personC = CoexistenceManager.i.GetRandomPerson();

            CurriculumData personA = CoexistenceManager.i.GetRandomPerson(personC);
            
            CurriculumData personB = CoexistenceManager.i.GetRandomPerson(personC, personA);

            string personAName = personA.personName + "\n";
            string personBName = personB.personName + "\n";
            QueueManager.i.AddQuestionPerson(personAName + " e " + personBName + " estão brigando feio", "Deixa lá", "Manda embora", ()=>{
                AddTemporaryStats(.15f,0f,30f);
            },()=>{
                CoexistenceManager.i.RemovePerson(personA);
                CoexistenceManager.i.RemovePerson(personB);
            },personC.TempCur());
        });
        #endregion

        #region Break Event
        AddEvent(breakEvents, ()=>
        {
            if(CoexistenceManager.i.GetPersons().Length < 2) return;
            CurriculumData owner = CoexistenceManager.i.GetRandomPerson();
            CurriculumData data = CoexistenceManager.i.GetRandomPerson(owner);

            QueueManager.i.AddQuestionPerson("O Computador "+ ((data.gender.ToLower()[0] == 'm') ? "da " : "do ") + data.personName + "\n parece quebrado", "Compra outro", "Deixa assim", ()=>{EarningSystem.i.ChangeMoney(-Random.Range(1500,3000),"Computador");}, ()=>{AddTemporaryStats(0f,0.05f,30f);},owner.TempCur());
            UpdateValue();
        });
        #endregion

        #region Random Event
        AddEvent(randomEvents, ()=>
        {
            if(CoexistenceManager.i.GetPersons().Length < 1) return;
            QueueManager.i.AddQuestionPerson("Nossos algorítmos detectaram que um criador de conteúdo produziu um vídeo utilizando um dos nossos projetos, acha que deveríamos agir?", "Sim", "Não", ()=>{
                EarningSystem.i.ChangeMoney(1500, "Copyright");
            }, ()=>{
                EarningSystem.i.ChangeMoney(Random.Range(-1000, 3000), "Fama");
            });
            UpdateValue();
        });

        AddEvent(randomEvents, ()=>
        {
            EarningSystem.i.ChangeMoney(Random.Range(100, 500), "Boas avaliações");

            UpdateValue();
        });

        AddEvent(randomEvents, ()=>
        {
            EarningSystem.i.ChangeMoney(-Random.Range(100, 500), "Reembolsos");

            UpdateValue();
        });

        AddEvent(randomEvents, ()=>
        {
            if(CoexistenceManager.i.GetPersons().Length < 1) return;
            CurriculumData data = CoexistenceManager.i.GetRandomPerson();
            QueueManager.i.AddQuestionPerson("Um festival de jogos se aproxima e tem um prêmio para o melhor jogo, deveriamos participar? O resultado não é imediato e nem garantido", "Sim", "Não", ()=>{
                EarningSystem.i.ChangeMoney(-3000, "Festival");
                PeriodTimer.Timer(60, ()=>{
                    if(Random.value * 100f < 35f)
                    {
                        QueueManager.i.AddReportPerson("Vencemos! Recebemos um bom dinheiro do festival", "Perfeito!", ()=>EarningSystem.i.ChangeMoney(6000, "Festival"));
                    }else{
                        QueueManager.i.AddReportPerson("Infelizmente nosso jogo não foi o suficiente. Talvez tenhamos conseguido alguma fama, ou não", "Uma pena");
                    }
                }, null, "Festival");
            }, ()=>{
                
            },data.TempCur());

            UpdateValue();
        });


        AddEvent(randomEvents, ()=>
        {
            if(CoexistenceManager.i.GetPersons().Length < 5) return;
            string contractText = "Recebemos um pedido de uma outra desenvolvedora, eles precisam de alguns funcionários, pode dar um bom dinheiro, eles querem:";
            int amount = Random.Range(1,4);
            List<CurriculumData> datas = new List<CurriculumData>();

            for (int i = 0; i < amount; i++)
            {
                CurriculumData data = CoexistenceManager.i.GetRandomPerson();

                datas.Add(data);

                contractText += "\n- " + data.personName;
            }

            PaperManager.i.AddContractPaper(contractText, ()=>{
                for (int i = 0; i < datas.Count; i++)
                {
                    CoexistenceManager.i.RemovePerson(datas[i]);
                }

                PeriodTimer.Timer(100, ()=>{
                    int moneyGained = 0;
                    for (int i = 0; i < datas.Count; i++)
                    {
                        int index = i;
                        CoexistenceManager.i.AddPerson(datas[index]);

                        moneyGained += Random.Range(1000, 5000);
                    }

                    EarningSystem.i.ChangeMoney(moneyGained,"Freelance");
                },null,"Freelance");
            });

            UpdateValue();
        });

        
        #endregion
    }

    public void HireEvent()
    {
        QueueManager.i.AddQuestionPerson("Hoje é dia de contratação, acredita que podemos adicionar alguem?", "Sim", "Não",()=>{
            HiringManager.i.StartHiring();
        }, null, bossCurriculum);
    }

    public void FireEvent()
    {
        EventController.i.eventIsOn = false;
        TimeManager.i.timeIsRunning = false;
        QueueManager.i.AddQuestionPerson("Temos que manter a empresa um local dinamico para todos, tem alguem que precisa sair?", "Sim", "Não",()=>{
            HiringManager.i.StartFiring();
            
        }, null, bossCurriculum);
    }
    
    public void TaxesEvent()
    {
        CurriculumData[] data = CoexistenceManager.i.GetPersons();

        int taxes = 500;

        for (int i = 0; i < data.Length; i++)
        {
            taxes += 100;
        }

        EarningSystem.i.ChangeMoney(-taxes, "Impostos");
    }

    public void SalaryEvent()
    {
        CurriculumData[] data = CoexistenceManager.i.GetPersons();

        int amount = 0;

        for (int i = 0; i < data.Length; i++)
        {
            amount += int.Parse(data[i].salary.Replace("R$", "").Replace(" ", ""));
        }

        EarningSystem.i.ChangeMoney(-amount, "Pagamento");
    }

    public void EarnEvent()
    {
        CurriculumData[] data = CoexistenceManager.i.GetPersons();

        int amount = 0;

        for (int i = 0; i < data.Length; i++)
        {
            float variance = data[i].vacancy.variance;
            amount += Mathf.RoundToInt((int.Parse(data[i].salary.Replace("R$", "").Replace(" ", ""))) * Random.Range(variance, 1f + variance));
        }

        EarningSystem.i.ChangeMoney(amount, "Lucro");
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
        float chance = Random.value;
        
        bool runEvent = false;
        Debug.Log(chance);
        if(chanceToBreak + tempIncrease <= chance)
        {
            StartCoroutine(TickDelay(breakEvents[Random.Range(0,breakEvents.Count)], Random.Range(0f,3f)));
            runEvent = true;
        }

        chance = Random.value;
        Debug.Log(chance);
        if(chanceToConflict + tempIncrease <= chance) 
        {
            StartCoroutine(TickDelay(conflictEvents[Random.Range(0,conflictEvents.Count)], Random.Range(0f,3f)));
            runEvent = true;
        }

        chance = Random.value;
        Debug.Log(chance);
        if(chanceToRandom + tempIncrease <= chance)
        {
            StartCoroutine(TickDelay(randomEvents[Random.Range(0,randomEvents.Count)], Random.Range(0f,3f)));
            runEvent = true;
        } 

        if(runEvent == false) tempIncrease += .1f; else tempIncrease = 0f;

    }

    private IEnumerator TickDelay(BaseEvent call, float timer)
    {
        yield return new WaitForSeconds(timer);
        call.CallEvent();
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

