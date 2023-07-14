using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class CupScorer : MonoBehaviour
{
    public static CupScorer cupScorer;

    public CupFill cup;         // need a cup to score
    public CupOrderSO order;    // Order to fufill 

    [SerializeField]
    private SpriteRenderer[] lines;      // Lines using the same id as the drink layers
    [SerializeField]
    private bool testUpdate = false;

    private void Awake()        // Making this a temporary singleton delete pls later
    {
        if(cupScorer == null)
        {
            cupScorer = this;
        }
    }

    void Start()
    {
        InitialGuideLines();
    }

    private void Update()
    {
        if(testUpdate)
            UpdateAllGuideLines();
    }

    private void InitialGuideLines()
    {
        float cur;
        float prev = 0;
        for(int i = 0; i < lines.Length; i++)
        {
            cur = order.ingredients[i];     // Get amount of ingredient
            if(cur == 0)
            {
                lines[i].enabled = false;
                lines[i].gameObject.SetActive(false);
            }
            else
            {
                SetGuidelineYVal(i, cur, prev);
            }
            prev += cur;
        }
    }

    /// <summary>
    /// Set guideline to expected value using the actual current toppings amount.
    /// </summary>
    /// <param name="id">The guideline to be updated.</param>
    public void UpdateGuideLine(int id)
    {
        if (id < 0 || id >= lines.Length)
            return;

        float prevAmount = cup.GetToppingSum(id - 1);
        SetGuidelineYVal(id, order.ingredients[id], prevAmount);
    }

    /// <summary>
    /// Update all guidelines using actual topping data.
    /// </summary>
    public void UpdateAllGuideLines()
    {
        for(int i = 0; i < lines.Length; i++)
        {
            UpdateGuideLine(i);
        }
    }

    public void HideAllButOne(int id)
    {
        for(int i = 0; i < lines.Length; i++)
        {
            Color c = lines[i].color;
            if (i != id)
            {   
                c.a = 0;  
            }
            else
            {
                c.a = 1;
            }
            lines[i].color = c;
        }
        UpdateGuideLine(id);
    }

    public void RevealAllUpdate()
    {
        foreach(SpriteRenderer sr in lines)
        {
            Color c = sr.color;
            c.a = 1;
            sr.color = c;
        }
        UpdateAllGuideLines();
    }

    public void RevealAllInitial()
    {
        foreach (SpriteRenderer sr in lines)
        {
            Color c = sr.color;
            c.a = 1;
            sr.color = c;
        }
        InitialGuideLines();
    }

    public float ScoreCup()
    {
        float inaccuracy = 0;       // Represents how different the expected cup is from the actual

        List<float> cupScores = cup.GetAllToppingAmounts();

        for(int i = 0; i < cupScores.Count; i++)
        {
            inaccuracy += Mathf.Abs(order.ingredients[i] - cupScores[i]);
        }

        return inaccuracy;
    }

    /// <summary>
    /// Sets the y-value of guideline id by summing the current amount of an ingredient + sum of previous ingredients, then converting value into a local position.
    /// </summary>
    /// <param name="id">Id of guideline to be set.</param>
    /// <param name="currentAmount">Current amount of ingredient for id line.</param>
    /// <param name="previousAmount">Amount of ingredient of all previous lines.</param>
    private void SetGuidelineYVal(int id, float currentAmount, float previousAmount)
    {
        float yVal = Mathf.Lerp(cup.minLevel, cup.maxLevel, currentAmount + previousAmount);      // Gets intended y-value of line
        Vector3 localPos = lines[id].transform.localPosition;
        lines[id].transform.localPosition = new Vector3(localPos.x, yVal, localPos.z);
    }
}
