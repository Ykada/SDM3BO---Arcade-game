using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiStarter : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canvas.gameObject.SetActive(true);
    }
}
