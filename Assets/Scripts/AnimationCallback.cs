using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public PlayerController pc;
    public AudioSource[] audio;

    // Start is called before the first frame update
    void Start()
    {
        if (pc == null)
        {
            Debug.LogWarning("No player controller to callback to");
        }
        audio = GetComponents<AudioSource>();
    }

    public void PlayWalkSound()
    {
        audio[0].Play();
    }

    public void PlayFlySound()
    {
        audio[1].Play();
    }

    public void PlayJumpSound()
    {
        if (!audio[2].isPlaying)
        {
            audio[2].Play();
        }
    }

    public void PlayLandSound()
    {
        audio[3].Play();
    }

    public void noPlayerControl()
    {
        pc.noPlayerControl();
    }

    public void playerControlOkay()
    {
        pc.playerControlOkay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
