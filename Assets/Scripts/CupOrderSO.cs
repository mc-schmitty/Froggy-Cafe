using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomOrder", menuName = "ScriptableObjects/CupOrderSO", order = 0)]
public class CupOrderSO : ScriptableObject
{
    [Range(0, 1)]
    public float[] ingredients = new float[10];
}
