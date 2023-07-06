using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTestButton : MonoBehaviour
{
    [SerializeField] CupFill testCup;
    [SerializeField] int id;
    [SerializeField] float pourRate;

    private void OnMouseDrag()
    {
        testCup.IncrementTopping(id, pourRate*Time.deltaTime);
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
