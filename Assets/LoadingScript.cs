using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;

public class UILoading : MonoBehaviour
{
    public Canvas loadSingCanvas;
    public Text loadingText;
    public float CompilingDuration = 1f;
    public float loadingDuration = 1f;
    public Canvas loadingCanvas;
    public Canvas MainUiCanvas;

    private void Start()
    {
        StartCoroutine(LoadAndDisplay());
    }

    private IEnumerator LoadAndDisplay()
    {
        loadSingCanvas.gameObject.SetActive(true);
        float timeElapsed = 0f;

        while (timeElapsed < CompilingDuration)
        {
            float progress = Mathf.Clamp01(timeElapsed / CompilingDuration);
            loadingText.text = (progress * 100f).ToString("F0") + "% Compiling Shaders";
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        float timeElapsedd = 0f;
        while (timeElapsedd < loadingDuration)
        {
            float progress = Mathf.Clamp01(timeElapsedd / loadingDuration);
            loadingText.text = (progress * 100f).ToString("F0") + "% Loading Game";
            timeElapsedd += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        loadingCanvas.gameObject.SetActive(false);
        MainUiCanvas.gameObject.SetActive(true);
    }
}
