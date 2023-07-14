using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupFill : MonoBehaviour
{
    public float maxLevel;     //Max height layer can reach before overflow
    public float minLevel;     // Starting height for layers

    [SerializeField]
    private SpriteRenderer[] toppings;


    private void Start()
    {
        ResetCupIngredients();
    }

    /// <summary>
    /// Resets all cup ingredients to 0% and empties the cup.
    /// </summary>
    public void ResetCupIngredients()
    {
        Vector3 startLvl = new Vector3(0, minLevel, 0);

        foreach (SpriteRenderer sr in toppings)      // Setup toppings
        {
            sr.transform.localPosition = startLvl;
            sr.enabled = false;
        }
    }

    /// <summary>
    /// Adds amount to the topping specified by id. Will raise all other toppings above topping id by that amount.
    /// </summary>
    /// <param name="id">int id of topping to increment.</param>
    /// <param name="amount">float amount of topping to be added.</param>
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
        Vector3 localPos = toppings[id].transform.localPosition;
        float newY = localPos.y + amount;
        if (newY >= maxLevel && toppings[id].enabled)       // Prevent overflow and notify if it happens
        {
            newY = maxLevel;
            // Call an overflow here!!!
            Debug.Log("Overflow with id " + id);
        }

        
        toppings[id].transform.localPosition = new Vector3(localPos.x, newY, localPos.z);    // Set Y to new level

        if(id+1 < toppings.Length)      // Make lighter layers flow up too
        {
            AddTopping(id + 1, amount);
        }
    }

    /// <summary>
    /// Gets the amount of topping inside the drink on a scale of 0 - 1.
    /// </summary>
    /// <param name="id">int id of the topping.</param>
    /// <returns></returns>
    public float GetToppingAmount(int id)
    {
        if (id < 0 || id >= toppings.Length)
            return 0;

        float yVal = toppings[id].transform.localPosition.y;
        float subY = id > 0 ? toppings[id - 1].transform.localPosition.y : minLevel;

        return (yVal - subY) / (maxLevel - minLevel);   // Essentially inverse lerping here
    }

    /// <summary>
    /// Returns a list containing all the toppings inside the drink on a scale of 0 - 1.
    /// </summary>
    /// <returns></returns>
    public List<float> GetAllToppingAmounts()
    {
        List<float> li = new List<float>();
        
        for(int i =0; i < toppings.Length; i++)
        {
            li.Add(GetToppingAmount(i));
        }

        return li;
    }

    /// <summary>
    /// Gets the amount of topping + amount of previous total topping before layer inside drink on a scale of 0 - 1.
    /// Essentially returns the height of the topping, converted to a percent of cup filled.
    /// </summary>
    /// <param name="id">int id of the topping.</param>
    /// <returns></returns>
    public float GetToppingSum(int id)
    {
        if (id < 0 || id >= toppings.Length)
            return 0;

        return Mathf.InverseLerp(minLevel, maxLevel, toppings[id].transform.localPosition.y);
    }
}
