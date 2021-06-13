using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip jump, enemy, coin;

    static AudioSource audioSource;

    void Start()
    {
        jump = Resources.Load<AudioClip>("jump");
        enemy = Resources.Load<AudioClip>("enemy");
        coin = Resources.Load<AudioClip>("coin");

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSource.PlayOneShot(jump);
                break;

            case "enemy":
                audioSource.PlayOneShot(enemy);
                break;
            
            case "coin":
                audioSource.PlayOneShot(coin);
                break;
        }
    }
}
