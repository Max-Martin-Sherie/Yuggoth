using System;
using TMPro;
using UnityEngine;

public class LanguageChange : MonoBehaviour
{
    [SerializeField] [TextArea] private String m_frenchText;
    [SerializeField] [TextArea] private String m_englishText;
    
    public delegate void OnLanguageChange();
    public static OnLanguageChange SwitchToEnglish;
    public static OnLanguageChange SwitchToFrench;
    public static bool m_french = true;

    private TextMeshProUGUI m_tmpText;

    private void Start()
    {
        SwitchToEnglish += SwitchLanguageEN;
        SwitchToFrench += SwitchLanguageFR;

        m_tmpText = GetComponent<TextMeshProUGUI>();

        if (m_french) SwitchLanguageFR();
        else SwitchLanguageEN();
    }

    private void SwitchLanguageFR()
    {
        m_tmpText.text = m_frenchText;
    }
    
    private void SwitchLanguageEN()
    {
        m_tmpText.text = m_englishText;
    }

    public void SwitchEng()
    {
        if(SwitchToEnglish != null)SwitchToEnglish();
    }
    
    public void SwitchFr()
    {
        if(SwitchToFrench != null)SwitchToFrench();
    }
}
