using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            SoundManager.PlaySound("coin");
            Destroy(gameObject);
        }
    }
}
