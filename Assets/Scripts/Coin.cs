using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // declaration
    // audio
    public AudioSource coinAudio;
    public AudioClip collectCoin;
    void Start()
    {
        StartCoroutine(Animate());
    }

    // movement
    private IEnumerator Animate()
    {
        // play audio
        coinAudio.PlayOneShot(collectCoin);
        // move box up and back down
        Vector3 originalPosition = transform.localPosition;
        Vector3 finalPosition = originalPosition + Vector3.up * 2f;
        yield return Move(originalPosition, finalPosition);
        yield return Move(finalPosition, originalPosition);

        // destroy coin after
        Destroy(gameObject);
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.25f;
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
