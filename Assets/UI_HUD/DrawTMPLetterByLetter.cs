using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class DrawTMPLetterByLetter : MonoBehaviour
{
    private TextMeshProUGUI m_text;

    [SerializeField] private float m_secondsBewtweenLetters = 0.2f;

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

        int i=0;
        while (text.Length > m_text.text.Length)
        {
            m_text.text += text[i];

            i++;
            yield return new WaitForSeconds(m_secondsBewtweenLetters);
        }

        Debug.Log("END");
    }
}