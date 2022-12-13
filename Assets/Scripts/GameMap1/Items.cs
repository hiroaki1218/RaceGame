using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Items
{
    //public GameObject item;
    //public string expalianText;

    //—ñ‹“Œ^Fí—Ş‚ğ’è‹`
    public enum Type
    {
        Toolbox,
        Recoveryoil,
        Newcar,
        RocketLauncher,
        Thumbtack,
        GuidedMissile,
        Digitalhack,
        StickyBomb,
        SoundCrackingSpeaker,
        Smoke,
        DOSAttack,
        GlassShield,
        TemperedGlassShield,
        SuperCushion,
        Nitro,
        DoubleNitro,
        SelfDriving,
        CheatCode,
        YearEndJumbo
    }

    //ItemType‚ğéŒ¾
    public Type type;
    //Item‰æ‘œ‚ğéŒ¾
    public Sprite sprite;
    public String itemname;

    public Items(Type type, Sprite sprite, String itemname)
    {
        this.type = type;
        this.sprite = sprite;
        this.itemname = itemname;
    }
}
