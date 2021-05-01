using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scan : PlayerCompetence
{
    [SerializeField] private GameObject m_scanner;

    //Durée du cooldown en seconde
    [SerializeField] private float m_cooldown;
    
    private bool m_scanActivated;



    // Start is called before the first frame update
    void Start()
    {
        m_scanActivated = false;
        m_scanner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       if(!m_scanActivated && Input.GetButtonDown("Scan"))
       {
            //Activation et reposition du scanner
            m_scanner.SetActive(true);
            m_scanner.transform.position = transform.position + new Vector3 (0f,0.5f,0f);

            m_scanActivated = true;
            m_scanner.GetComponent<ScanVerification>().m_startScan = true;
            StartCoroutine(Cooldown());
       }

    }

    private IEnumerator Cooldown()
    {
        //Cooldown de l'utilisation du scanner
        yield return new WaitForSeconds(m_cooldown);
        m_scanActivated = false;
    }


}
