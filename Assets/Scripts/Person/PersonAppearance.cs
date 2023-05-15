using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAppearance : MonoBehaviour
{
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer neck;
    [SerializeField] private SpriteRenderer glasses;
    [SerializeField] private SpriteRenderer nose;
    [SerializeField] private SpriteRenderer mouth;
    [SerializeField] private SpriteRenderer clothes;
    [SerializeField] private SpriteRenderer hair;

    public void SetAppearance(PersonInfo info)
    {
        bool isAWoman = (info.c.gender.ToLower()[0] == 'm');

        transform.position = new Vector3(transform.position.x, transform.position.y + info.c.height / 100f, transform.position.z);
        head.sprite = isAWoman ? InformationDatabase.i.feminine_heads[info.c.headType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        
        Debug.Log(info.c.skinHex);

        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out Color skinColor)) head.color = skinColor;
        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out skinColor)) neck.color = skinColor;
        

        hair.sprite = isAWoman ? InformationDatabase.i.feminine_hairs[info.c.hairType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        if(ColorUtility.TryParseHtmlString(info.c.hairHex, out Color hairColor)) hair.color = hairColor;
        
        
        glasses.sprite = isAWoman ? InformationDatabase.i.feminine_glasses[info.c.glassesType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        nose.sprite = isAWoman ? InformationDatabase.i.feminine_noses[info.c.noseType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        mouth.sprite = isAWoman ? InformationDatabase.i.feminine_mouths[info.c.mouthType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        
        clothes.sprite = isAWoman ? InformationDatabase.i.feminine_clothes[info.c.clothesType] : InformationDatabase.i.masculineBodies[info.c.bodyType];
        if(ColorUtility.TryParseHtmlString(info.c.clothesHex, out Color clothesColor)) clothes.color = clothesColor;
        
    
    }
}
