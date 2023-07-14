using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StrawStabber : MonoBehaviour
{
    public float activateStrawYBound;
    public float piercingYBound;
    public float releaseYBound;
    public float xBounds;
    public float minPiercingVelocity;
    public float strawFinalY;
    public float strawTimeToFinal = 1;

    [SerializeField]
    private VelocityChecker vc;
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    int state = 0;
    Vector2 initPos;
    Camera mainCam;

    private void Start()
    {
        initPos = transform.position;
        mainCam = Camera.main;
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        switch (state)
        {
            case 0:
                transform.position = mousePos;

                if (transform.localPosition.y > activateStrawYBound)
                {
                    state = 1;
                    vc.enabled = true;
                    // trigger any other transitions
                }
                break;
            case 1:
                transform.position = mousePos;

                if(transform.localPosition.y < piercingYBound)
                {
                    Vector2 vel = vc.GetVelocitySum();
                    Debug.Log(vel);
                    if(Mathf.Abs(transform.localPosition.x) < xBounds && -(vel.y) > minPiercingVelocity)
                    {
                        // succeed stab
                        state = 2;
                        sr.sortingOrder = 2;
                    }
                    else
                    {
                        // fail stab
                        state = 0;
                    }
                    vc.enabled = false;

                }
                break;
            case 2:
                transform.position = new Vector2(transform.position.x, mousePos.y);

                if(transform.localPosition.y > piercingYBound || transform.localPosition.y < releaseYBound)
                {
                    state = 3;
                    // and then trigger coroutine to move straw to point
                    StartCoroutine(MoveStrawToMiddle(strawFinalY, strawTimeToFinal));
                }
                break;
        }
    }

    private void OnMouseUp()
    {
        if(state < 2)
        {
            transform.position = initPos;
            state = 0;
            vc.enabled = false;
        }
        else if(state == 2)
        {
            state = 3;
            StartCoroutine(MoveStrawToMiddle(strawFinalY, strawTimeToFinal));        // lerp straw to pos
        }
    }

    IEnumerator MoveStrawToMiddle(float yMid, float timeToReachMid)
    {
        Vector2 startPos = transform.localPosition;
        Vector2 endPos = new Vector2(transform.localPosition.x, yMid);

        float x = 0;
        while(x < timeToReachMid)
        {
            transform.localPosition = Vector2.Lerp(startPos, endPos, x/timeToReachMid);
            x += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            Vector3 avgV = (Vector3)vc.GetVelocitySum();
            //Debug.Log(avgV);
            Gizmos.DrawLine(transform.position, transform.position + avgV);
        }
        catch(System.Exception e)
        {

        }
    }
}
