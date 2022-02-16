using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource flauta;
    // Start is called before the first frame update
    void Start()
    {
        flauta = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flauta.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            flauta.Pause();
        }
    }
}
