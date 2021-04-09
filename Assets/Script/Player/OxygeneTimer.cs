using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygeneTimer : MonoBehaviour
{
    public Slider m_timerSlider;

    //m_oxygeneTimer correspond a la dur�e que le joueur peut parcourir en minutes.
    public float m_oxygeneTimer;
    //m_timer correspond au moment o� le joueur n'aura plus d'oxyg�ne.
    private float m_timer;

    [HideInInspector]
    public bool m_stopTimer;

    private PlayerController m_playerController;
    private CameraController m_cameraController;


    //m_onO2 est un booll�en qui v�rifie si le joueur est dans une zone, ou fais une action, o� il peut r�cup�rer l'oxyg�ne.
    private bool m_onO2;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();
        m_cameraController = GetComponent<CameraController>();

        m_onO2 = false;

        m_oxygeneTimer *= 60;

        m_timerSlider.maxValue = m_oxygeneTimer;
        m_timerSlider.value = m_oxygeneTimer;

        m_timer = m_oxygeneTimer + Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        float time = m_timer - Time.time;

        if (time <= 0)
        {
            m_playerController.m_moveDir = new Vector3(0f, 0f, 0f);
            m_cameraController.m_mouseSensitivity = 0f;
        }

        if (!m_stopTimer)
        {
            m_timerSlider.value = time;
        }

        if (m_onO2)
        {
            OxygeneRegeneration();
        }

    }

    private void OnCollisionEnter(Collision p_other)
    {
        if (p_other.gameObject.CompareTag("O2"))
            m_onO2 = true;
        else
            m_onO2 = false;
    }

    public void OxygeneRegeneration()
    {
        m_timer = m_oxygeneTimer + Time.time;
    }

}
