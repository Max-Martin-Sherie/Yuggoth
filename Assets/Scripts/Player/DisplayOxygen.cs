using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOxygen : MonoBehaviour
{
    [SerializeField] Transform m_targetPoint;
    [SerializeField] float m_MinStartDistance = 2f;
    [SerializeField] float m_flashSpeed = 1f;
    [SerializeField] Text m_text ;

    private bool m_startFlashing = false;

    IEnumerator OxygenFlash()
    {
        while (true)
        {
            m_text.enabled = !m_text.enabled;

            yield return new WaitForSeconds(m_flashSpeed);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!m_startFlashing && Vector3.Distance(transform.position, m_targetPoint.position) < m_MinStartDistance)
        {
            m_startFlashing = true;
            StartCoroutine(OxygenFlash());
        }
    }
}
