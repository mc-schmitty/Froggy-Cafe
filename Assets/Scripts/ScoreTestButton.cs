using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTestButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Score: " + CupScorer.cupScorer.ScoreCup());
    }
}
