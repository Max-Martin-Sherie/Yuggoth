using System;
using System.Collections;
using UnityEngine;

public class ResetTranslate : MonoBehaviour
{
    [SerializeField] private bool m_enviro1 = false;
    [SerializeField] private GameObject m_introCanvas;
    
    // Start is called before the first frame update
    void Awake()
    {

        LanguageChange.SwitchToEnglish = null;
        LanguageChange.SwitchToFrench = null;

        LanguageChange.SwitchToEnglish += SwitchBoolEng;
        LanguageChange.SwitchToFrench += SwitchBoolFR;
    }

    private void Start()
    {
        StartCoroutine(CallBool());
    }

    IEnumerator CallBool()
    {
        yield return new WaitForSeconds(.2f);
        if(m_enviro1) m_introCanvas.SetActive(true);
        Debug.Log(LanguageChange.m_french);
        
        yield return new WaitForSeconds(.1f);
        
        if(LanguageChange.m_french) LanguageChange.SwitchToFrench();
        else LanguageChange.SwitchToEnglish();
    }
    
    void SwitchBoolFR()
    {
        LanguageChange.m_french = true;
    }

    void SwitchBoolEng()
    {
        LanguageChange.m_french = false;
    }
}
