using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = AudioListener.volume;
    }

    public void SetNewVolume(float p_volume)
    {
        AudioListener.volume = p_volume;
    } 
}
