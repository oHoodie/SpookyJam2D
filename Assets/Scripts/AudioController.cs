using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float transitionSpeed;
    public AudioSource mainAmbient;
    public AudioSource closeAmbient;
    public AudioSource chasedAmbient;

    private AudioSource transitionToChase;
    private float chaseCounter = 0;
    private float minChaseThemeTime = 0;


    public enum AudioState
    {
        Normal, Close, Chased
    }

    // 1 AudioState per enemy
    private AudioState[] audioStates = new AudioState[] { AudioState.Normal, AudioState.Normal };
    private AudioState currentAudioState = AudioState.Normal;

    public void SetAudioState(AudioState state, string monsterName)
    {
        int index = monsterName == "Curupira" ? 0 : 1;
        audioStates[index] = state;

        // Play Chase starts sound effect
        if ((audioStates[0] == AudioState.Chased || audioStates[1] == AudioState.Chased) && currentAudioState != AudioState.Chased && chaseCounter <= 0)
        {
            transitionToChase.Play();
            chaseCounter = 20;
        }

        if(minChaseThemeTime <= 0)
        {
            // Set overall chase state
            if (audioStates[0] == AudioState.Chased || audioStates[1] == AudioState.Chased)
            {
                currentAudioState = AudioState.Chased;
                minChaseThemeTime = 10;
            }
            else if (audioStates[0] == AudioState.Close || audioStates[1] == AudioState.Close)
            {
                currentAudioState = AudioState.Close;
            }
            else
            {
                currentAudioState = AudioState.Normal;
            }
        }


    }
    


    // Start is called before the first frame update
    void Start()
    {
        transitionToChase = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        minChaseThemeTime -= Time.deltaTime;
        chaseCounter -= Time.deltaTime;

        switch (currentAudioState)
        {
            case AudioState.Normal:
                mainAmbient.volume = Mathf.Lerp(mainAmbient.volume, .15f, transitionSpeed);
                closeAmbient.volume = Mathf.Lerp(closeAmbient.volume, 0, transitionSpeed);
                chasedAmbient.volume = Mathf.Lerp(chasedAmbient.volume, 0, transitionSpeed);
                break;
            case AudioState.Close:
                mainAmbient.volume = Mathf.Lerp(mainAmbient.volume, .15f, transitionSpeed);
                closeAmbient.volume = Mathf.Lerp(closeAmbient.volume, .15f, transitionSpeed);
                chasedAmbient.volume = Mathf.Lerp(chasedAmbient.volume, 0, transitionSpeed);
                break;
            case AudioState.Chased:
                mainAmbient.volume = Mathf.Lerp(mainAmbient.volume, .15f, transitionSpeed);
                closeAmbient.volume = Mathf.Lerp(closeAmbient.volume, 0, transitionSpeed);
                chasedAmbient.volume = Mathf.Lerp(chasedAmbient.volume, .15f, transitionSpeed);
                break;
        }
    }

}
