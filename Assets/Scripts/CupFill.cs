using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFill : MonoBehaviour
{
    [SerializeField]
    private float maxLevel;     //Max height layer can reach before overflow
    [SerializeField]
    private float minLevel;     // Starting height for layers

    [SerializeField]
    private SpriteRenderer[] toppings;


    private void Start()
    {
        Vector3 startLvl = new Vector3(0, minLevel, 0);

        foreach(SpriteRenderer sr in toppings)      // Setup toppings
        {
            sr.transform.localPosition = startLvl;
            sr.enabled = false;
        }
    }

    public void IncrementTopping(int id, float amount)
    {
        if(id < toppings.Length)        // Make sure topping exists
        {
            if (!toppings[id].enabled)       // Enable topping
                toppings[id].enabled = true;

            AddTopping(id, amount);
        }
    }

    private void AddTopping(int id, float amount)
    {
        float newY = toppings[id].transform.localPosition.y + amount;
        if (newY >= maxLevel && toppings[id].enabled)
        {
            newY = maxLevel;
            // Call an overflow here!!!
            Debug.Log("Overflow with id " + id);
        }

        toppings[id].transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);    // Set Y to new level

        if(id+1 < toppings.Length)      // Make lighter layers flow up too
        {
            AddTopping(id + 1, amount);
        }
    }
}
