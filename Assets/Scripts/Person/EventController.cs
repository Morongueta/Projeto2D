using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    //Lista de eventos de cada tipo
    //-Coisas quebrando
    //-Conflito
    //-Aleatório
    //-Governamental (Fiscal, imposto, essas coisas)
    //-Necessidades (Ausencia de Faxineiro, Segurança, Gerente)
}


public class BaseEvent
{
    public virtual void CallEvent()
    {

    }
}
