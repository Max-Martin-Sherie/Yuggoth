using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanVerification : MonoBehaviour
{
    #region private

    //Scan's range
    [Range(100, 1000)] [SerializeField] private int m_scanRange;
    private Vector3 m_initScale = new Vector3(0f, 0.5f, 0f);

    //Verification du Line renderer
    private bool m_activeLine = false;

    //GameObject safe zone
    private GameObject m_oxPl;

    #endregion

    public bool m_startScan = false;


    void Start()
    {
        transform.localScale = m_initScale;
    }


    void Update()
    {
        if (m_startScan && transform.localScale.x != m_scanRange)
            transform.localScale += new Vector3(0.5f, 0f, 0.5f);


        if (transform.localScale.x == m_scanRange)
        {
            transform.localScale = m_initScale;
            
            m_startScan = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //Vérification de l'initialisation du Line Renderer pour n'afficher que la safe zone la plus proche
        if (!m_activeLine && other.gameObject.CompareTag("O2"))
        {
            m_oxPl = other.gameObject;
            m_oxPl.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 350f, 0f));
            m_activeLine = true;
            StartCoroutine(LineCooldown());
        }
    }

    private IEnumerator LineCooldown()
    {
        //Cooldown avant la fin d'affichage du Line Renderer
        yield return new WaitForSeconds(5);
        m_oxPl.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, 0f));
        m_oxPl = null;
        transform.gameObject.SetActive(false);
        m_activeLine = false;
    }
}
