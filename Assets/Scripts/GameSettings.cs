using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int[] mStagesKnifeCount = { 7, 9, 10, 8 , 7};
    public float mWheelSpeed = 100f;
    public int mAppleDropChance = 25;
}
