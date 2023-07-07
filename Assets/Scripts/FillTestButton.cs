using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTestButton : MonoBehaviour
{
    [SerializeField] CupFill testCup;
    [SerializeField] int id;
    [SerializeField] float pourRate;

    private void OnMouseDown()
    {
        CupScorer.cupScorer.HideAllButOne(id);
    }

    private void OnMouseDrag()
    {
        testCup.IncrementTopping(id, pourRate*Time.deltaTime);
        //transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void OnMouseUp()
    {
        Debug.Log(testCup.GetToppingAmount(id)*100 + "%");
        //CupScorer.cupScorer.RevealAllUpdate();
    }
}
