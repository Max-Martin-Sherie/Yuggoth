using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSensitivity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = CameraController.m_mouseSensitivity;
    }

    // Update is called once per frame
    public void SetNewSensitivity(float p_newSens)
    {
        CameraController.m_mouseSensitivity = p_newSens;
    }
}
