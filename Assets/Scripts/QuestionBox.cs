using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    // declaration
    public bool hasCoin;
    private bool moving;

    // coin
    public GameObject coin;

    // animator
    public Animator questionBlockAnimator;

    void Start()
    {
        hasCoin = true;
    }
    // detect if mario collides with the block
    private void OnCollisionEnter2D(Collision2D col)
    {

        // check if it is mario
        if (!moving && col.gameObject.CompareTag("Player"))
        {
            // check if mario is hitting the question box from below
            if (col.transform.position.y < transform.position.y)
            {
                // call hit function
                Hit();
            }
        }
    }

    private void Hit()
    {
        // check coin state of box
        if (hasCoin)
        {
            // do movement
            StartCoroutine(Animate());
            // update coin state to false
            hasCoin = false;
            // update animation
            questionBlockAnimator.SetBool("hasCoin", hasCoin);
            // instantiate coin
            // check if coin is not null
            if (coin != null)
            {
                Instantiate(coin, transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator Animate()
    {
        moving = true;
        // move box up and back down
        Vector3 originalPosition = transform.localPosition;
        Vector3 finalPosition = originalPosition + Vector3.up * 0.5f;
        yield return Move(originalPosition, finalPosition);
        yield return Move(finalPosition, originalPosition);
        moving = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = to;
    }
}
