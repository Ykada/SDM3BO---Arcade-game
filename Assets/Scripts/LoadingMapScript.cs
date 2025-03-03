using UnityEngine;
using UnityEngine.UI;  // If you're working with UI elements like buttons, text, etc.

public class DisableUIAfterRandomTime : MonoBehaviour
{
    public GameObject uiElement;
    public float minTime = 1f;
    public float maxTime = 5f;
    public GameObject player;

    private float randomTime;
    private float timer;

    void Start()
    {
        randomTime = Random.Range(minTime, maxTime);
        timer = 0f;
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            DisableUI();
        }
    }

    // Disable the UI element
    void DisableUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
            player.SetActive(true);
        }
    }
}
