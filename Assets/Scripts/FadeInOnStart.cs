using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnStart : MonoBehaviour
{
    [SerializeField] private RawImage m_blackScreen;
    [SerializeField] private bool m_removeText = false;
    [SerializeField] private TextMeshProUGUI m_text;

    [SerializeField] private float m_speed = 1f;
    [SerializeField] private float m_startDelay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        if(Time.realtimeSinceStartup <= 5f)yield return new WaitForSeconds(m_startDelay);
        
        while (m_blackScreen.color.a >= 0.001f)
        {
            m_blackScreen.color = Color.Lerp(m_blackScreen.color,new Color(m_blackScreen.color.r,m_blackScreen.color.g,m_blackScreen.color.b, 0f), Time.deltaTime * m_speed);
            if(m_removeText)m_text.color = m_blackScreen.color;
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
        
        Destroy(m_blackScreen.gameObject);
    }
}
