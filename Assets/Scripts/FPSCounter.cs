using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    public Text fpsText;
    public float updateInterval = 0.5f;

    private float timePassed = 0f;
    private int frameCount = 0;

    void Update()
    {
        frameCount++;
        timePassed += Time.deltaTime;
        if (timePassed >= updateInterval)
        {
            float fps = frameCount / timePassed;
            fpsText.text = "FPS: " + Mathf.FloorToInt(fps);
            timePassed = 0f;
            frameCount = 0;
        }
    }
}
