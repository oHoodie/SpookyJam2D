using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float transitionSpeed;
    public AudioSource mainAmbient;
    public AudioSource bassAmbient;

    public enum AudioState
    {
        Normal, Chased
    }

    private AudioState audioState = AudioState.Normal;

    public void SetAudioState(AudioState state)
    {
        audioState = state;
    }
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetAudioState(AudioState.Chased);
        }

        switch (audioState)
        {
            case AudioState.Normal:
                mainAmbient.volume = Mathf.Lerp(mainAmbient.volume, 1, transitionSpeed);
                bassAmbient.volume = Mathf.Lerp(bassAmbient.volume, 0, transitionSpeed);
                break;
            case AudioState.Chased:
                mainAmbient.volume = Mathf.Lerp(mainAmbient.volume, 1, transitionSpeed);
                bassAmbient.volume = Mathf.Lerp(bassAmbient.volume, 1, transitionSpeed);
                break;
        }
    }

}
