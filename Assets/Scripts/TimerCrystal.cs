using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCrystal : MonoBehaviour
{

    AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectMe()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false; //.SetActive(false);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<AudioSource>().Play();
        ParticleSystem system = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        system.gameObject.SetActive(true);
        system.Play();
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}
