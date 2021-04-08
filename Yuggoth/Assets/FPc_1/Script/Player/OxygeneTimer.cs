using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygeneTimer : MonoBehaviour
{
    public Slider m_timerSlider;

    //m_oxygeneTimer correspond a la durée que le joueur peut parcourir en minutes.
    public float m_oxygeneTimer;
    //m_timer correspond au moment où le joueur n'aura plus d'oxygène.
    private float m_timer;

    [HideInInspector]
    public bool m_stopTimer;

    //m_onO2 est un boolléen qui vérifie si le joueur est dans une zone, ou fais une action, où il peut récupérer l'oxygène.
    private bool m_onO2;

    private void Start()
    {
        m_stopTimer = false;
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
            m_stopTimer = true;
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
