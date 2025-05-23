using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class StaticObject : MonoBehaviour
{
    private UnsignedIntegerField mFlags;


    enum Flag
    {
        WALL = (1 << 0),
        BRICK = (1 << 1),
        BOMB = (1 << 2 ),
        ITEM_BOMB = (1 << 3),
        ITEM_POWER = (1 << 4 ),
        FIRE_X = (1 << 5 ),
        FIRE_Y = (1 << 6),
        EXPLODING = (1 << 7),
    };

    public void setFlag(UnsignedIntegerField unsigned)
    {
        mFlags.value |= unsigned.value;
    }
    public void restFlag(UnsignedIntegerField unsigned)
    {
        mFlags.value &= ~unsigned.value;
    }

    public void drawImage(int x, int y)
    {
        
    }
}
