using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    private Light2D spotlight;

    private bool isTakingPicture;
    private float picAnimCounter;

    private PlaySoundFromList camSound;

    // Start is called before the first frame update
    void Start()
    {
        spotlight = GetComponentInChildren<Light2D>();
        camSound = GetComponent<PlaySoundFromList>();
    }

    // Update is called once per frame
    void Update()
    {
        // Take a picture
        if (Input.GetKeyDown(KeyCode.Space) && !isTakingPicture)
        {
            isTakingPicture = true;
            camSound.PlayRandom();
            TurnOn(true);
        }

        HandleTakePicture();
        

    }

    private void HandleTakePicture()
    {
        if (isTakingPicture)
        {
            picAnimCounter += Time.deltaTime;
            if (picAnimCounter >= 0.4f)
            {
                spotlight.pointLightInnerAngle = 75;
            }
            if (picAnimCounter >= 1f)
            {
                TurnOn(false);
                picAnimCounter = 0;
                spotlight.pointLightInnerAngle = 0;
                isTakingPicture = false;
            }
        }
    }

    private void RayCastTarget()
    {

    }

    private void TurnOn(bool value)
    {
        spotlight.enabled = value;
    }
}
