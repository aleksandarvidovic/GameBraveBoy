using UnityEngine;
using UnityEngine.UI;

public class LifeNumber : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = Character.life.ToString();
    }
}
