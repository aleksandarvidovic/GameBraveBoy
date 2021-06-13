using System.Collections;
using UnityEngine;

public class Metal : MonoBehaviour
{
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] public float killX;
    [SerializeField] public float killY;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(PosReset());
    }

    IEnumerator PosReset()
    {
        yield return new WaitForSeconds(2);
        rb.transform.position = new Vector3(x, y);
    }
}
