using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    private Light2D spotlight;

    // Start is called before the first frame update
    void Start()
    {
        spotlight = GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnOn(!spotlight.enabled);
        }
    }

    private void TurnOn(bool value)
    {
        spotlight.enabled = value;
    }
}
