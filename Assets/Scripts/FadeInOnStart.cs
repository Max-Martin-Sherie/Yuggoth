using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnStart : MonoBehaviour
{
    [SerializeField] private RawImage m_blackScreen;

    [SerializeField] private float m_speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        while (m_blackScreen.color.a >= 0.001f)
        {
            m_blackScreen.color = Color.Lerp(m_blackScreen.color,new Color(m_blackScreen.color.r,m_blackScreen.color.g,m_blackScreen.color.b, 0f), Time.deltaTime * m_speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        Destroy(m_blackScreen.gameObject);
    }
}
