using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(FallTimer());
    }

    IEnumerator FallTimer()
    {
        yield return new WaitForSeconds(0.5f);
        rb.mass = 1;
        rb.gravityScale = 1;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
