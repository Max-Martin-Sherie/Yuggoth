using System;
using System.Collections;
using UnityEngine;
using TMPro;

using Random = UnityEngine.Random;

public class DrawTMPLetterByLetter : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    [SerializeField] private float m_secondsBewtweenLetters = 0.2f;
    private float m_offset = 0;
    
    [SerializeField] private GameObject m_HUDIsActive;

    [SerializeField] private PlayerMove m_pm;

    [SerializeField] private RotateObject m_ro;
    
    private float m_speed;
    
    [SerializeField] private CameraController m_cc;

    private float m_sensitivity;
    
    
    [SerializeField] private Canvas m_canvasToDisable;
    private bool m_finished = false;

    [SerializeField] private AudioSource m_as;
    [SerializeField] private AudioSource m_asAmbient;
    
    // Start is called before the first frame update
    void Start()
    {
        m_ro.enabled = false;
        m_text = GetComponent<TextMeshProUGUI>();


        Debug.Log(m_text.text[0]);

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        m_speed = m_pm.m_moveSpeed;
        m_pm.m_moveSpeed = 0;

        m_pm.m_canJump = false;

        m_sensitivity = m_cc.m_mouseSensitivity;
        m_cc.m_mouseSensitivity = 0;
        String text = m_text.text;

        m_text.text = "";
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_HUDIsActive.SetActive(true);
        }

        int i=0;
        while (text.Length > m_text.text.Length)
        {
            m_text.text += text[i];

            m_offset = Random.Range(0,m_secondsBewtweenLetters / 2);
            
            
            m_as.Play();

            i++;
            yield return new WaitForSeconds(m_secondsBewtweenLetters + m_offset);
        }


        m_finished = true;
    }
    
    private void Update()
    {
        if (m_finished && Input.GetButtonDown("Fire1"))
        {
            m_text.color = new Color(0,0,0,0);
            m_canvasToDisable.enabled = false;
            m_HUDIsActive.SetActive(true);
            
            m_pm.m_moveSpeed = m_speed;
            m_pm.m_canJump = true;
            m_ro.enabled = true;
            m_cc.m_mouseSensitivity = m_sensitivity;
            m_asAmbient.Play();
            Destroy(this);
        }
    }
}