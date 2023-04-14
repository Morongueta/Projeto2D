using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAppearance : MonoBehaviour
{
    [SerializeField]private SpriteRenderer body;

    public void SetAppearance(PersonInfo info)
    {

        transform.position = new Vector3(transform.position.x, transform.position.y + info.c.height / 100f, transform.position.z);
        body.sprite = (info.c.gender.ToLower()[0] == 'm') ? InformationDatabase.i.feminineBodies[info.c.bodyType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
    }
}
