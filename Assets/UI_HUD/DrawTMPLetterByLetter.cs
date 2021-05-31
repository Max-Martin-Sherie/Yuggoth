using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DrawTMPLetterByLetter : MonoBehaviour
{
    private TextMeshProUGUI m_text;
    [SerializeField] private float m_secondsBewtweenLetters = 0.2f;
    
    [SerializeField] private Canvas m_DisabledCanvas;
    [SerializeField] private GameObject m_HUDisActive;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();


        Debug.Log(m_text.text[0]);

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        String text = m_text.text;

        m_text.text = "";
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_DisabledCanvas.GetComponent<Canvas> ().enabled = false;
            m_HUDisActive.SetActive(true);
        }

        int i=0;
        while (text.Length > m_text.text.Length)
        {
            m_text.text += text[i];

            i++;
            yield return new WaitForSeconds(m_secondsBewtweenLetters);
        }

       

        Debug.Log("END");
        
        //Disable intro canvas && enable HUD game 
        m_DisabledCanvas.GetComponent<Canvas> ().enabled = false;
        m_HUDisActive.SetActive(true);
    
       
        

    }
}