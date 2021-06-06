using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayOxygen : MonoBehaviour
{
    [SerializeField] Transform m_targetPoint;
    [SerializeField] float m_MinStartDistance = 2f;
    [SerializeField] float m_flashSpeed = 1f;
    [SerializeField] GameObject m_alertImage ;
    [SerializeField] GameObject m_oldDisplay ;

    private bool m_startFlashing = false;

    private bool active = true;

    IEnumerator OxygenFlash()
    {
        m_oldDisplay.SetActive(false);
        while (true)
        {
            active = !active;
            m_alertImage.SetActive(active);

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
