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

    private void Start()
    {
        SwitchToEnglish += SwitchEng;
        SwitchToFrench += SwitchFr;
    }

    private void SwitchLanguageFR()
    {
        GetComponent<TextMeshProUGUI>().text = m_frenchText;
    }
    
    private void SwitchLanguageEN()
    {
        GetComponent<TextMeshProUGUI>().text = m_englishText;
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
