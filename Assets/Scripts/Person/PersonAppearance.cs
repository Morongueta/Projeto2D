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
    [SerializeField] private SpriteRenderer facialHair;
    [SerializeField] private SpriteRenderer ears;
    [SerializeField] private SpriteRenderer custom;
    [SerializeField] private Sprite customAppearance;

    public void SetAppearance(PersonInfo info)
    {
        if(info.c.customSprite != null)
        {
            custom.sprite = info.c.customSprite;
            return;
        }
        custom.sprite = null;

        bool isAWoman = (info.c.gender.ToLower()[0] == 'm');

        transform.position = new Vector3(transform.position.x, transform.position.y + info.c.height / 100f, transform.position.z);
        head.sprite = isAWoman ? InformationDatabase.i.feminine_heads[info.c.headType] : InformationDatabase.i.masculine_head;

        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out Color skinColor)) head.color = skinColor;
        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out skinColor)) neck.color = skinColor;
        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out skinColor)) nose.color = skinColor;
        if(ColorUtility.TryParseHtmlString(info.c.skinHex, out skinColor)) ears.color = skinColor;
        

        hair.sprite = isAWoman ? InformationDatabase.i.feminine_hairs[info.c.hairType] : InformationDatabase.i.masculine_hairs[info.c.hairType];
        facialHair.sprite = isAWoman ? null : InformationDatabase.i.masculine_facialHairs[info.c.mouthType];
        if(ColorUtility.TryParseHtmlString(info.c.hairHex, out Color hairColor)) hair.color = hairColor;
        if(ColorUtility.TryParseHtmlString(info.c.hairHex, out hairColor)) facialHair.color = hairColor;
        
        
        glasses.sprite = isAWoman ? InformationDatabase.i.feminine_glasses[info.c.glassesType] : InformationDatabase.i.masculine_glasses[info.c.glassesType];
        nose.sprite = isAWoman ? InformationDatabase.i.feminine_noses[info.c.noseType] : InformationDatabase.i.masculine_noses[info.c.noseType];
        mouth.sprite = isAWoman ? InformationDatabase.i.feminine_mouths[info.c.mouthType] : InformationDatabase.i.masculine_mouth;
        
        clothes.sprite = isAWoman ? InformationDatabase.i.feminine_clothes[info.c.clothesType] : InformationDatabase.i.masculine_clothes[info.c.clothesType];
        if(ColorUtility.TryParseHtmlString(info.c.clothesHex, out Color clothesColor)) clothes.color = clothesColor;
        
    
    }
}
