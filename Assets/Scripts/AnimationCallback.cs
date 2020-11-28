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
        if (audio != null && audio.Length > 0)
        {
            audio[0].Play();
        }
    }

    public void PlayFlySound()
    {
        if (audio != null && audio.Length > 1)
        {
            audio[1].Play();
        }
    }

    public void PlayLandSound()
    {
        if (audio != null && audio.Length > 2)
        {
            audio[2].Play();
        }
    }

    public void noPlayerControl()
    {
        if (pc != null)
        {
            pc.noPlayerControl();
        }
    }

    public void playerControlOkay()
    {
        if (pc != null)
        {
            pc.playerControlOkay();
        }
    }
}
