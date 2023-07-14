using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityChecker : MonoBehaviour
{
    public float period = 0.25f;

    Queue<Vector2> positions;
    Vector3 previousPos;

    void Start()
    {
        positions = new Queue<Vector2>();
        previousPos = transform.position;
    }

    public Vector2 GetVelocitySum()
    {
        Vector2 output = Vector2.zero;
        foreach(Vector2 v in positions)
        {
            output += v;
        }

        return output;
    }

    private void OnEnable()
    {
        previousPos = transform.position;
    }

    private void OnDisable()
    {
        positions.Clear();
    }

    private void FixedUpdate()
    {
        positions.Enqueue(transform.position - previousPos);
        previousPos = transform.position;

        if(positions.Count * Time.fixedDeltaTime > period)
        {
            positions.Dequeue();
        }
    }
}
