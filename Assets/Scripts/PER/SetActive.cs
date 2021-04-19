using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_activatorObjects = new List<GameObject>();
    public List<Receiver> m_receiversScripts = new List<Receiver>();
    [SerializeField] private List<GameObject> m_activablePlatforms = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_activatorObjects.Count; i++)
        {
            m_receiversScripts.Add(m_activatorObjects[i].GetComponent<Receiver>());
        }
        
        //Set active false platforms 
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
         0 = pink
         1 = blue 
         2 = purple 
         3 = orange
         */
        if (m_receiversScripts[0].m_lasered)
        {
            m_activablePlatforms[0].SetActive(true);
            
        } else if (!m_receiversScripts[0].m_lasered)
        {
            m_activablePlatforms[0].SetActive(false);
        }
        
        if (m_receiversScripts[1].m_lasered)
        {
            m_activablePlatforms[1].SetActive(true);
            
        } else if (!m_receiversScripts[1].m_lasered)
        {
            m_activablePlatforms[1].SetActive(false);
        }
        
        if (m_receiversScripts[2].m_lasered)
        {
            m_activablePlatforms[2].SetActive(true);
            
        } else if (!m_receiversScripts[2].m_lasered)
        {
            m_activablePlatforms[2].SetActive(false);
        }
        
        if (m_receiversScripts[3].m_lasered)
        {
            m_activablePlatforms[3].SetActive(true);
            
        } else if (!m_receiversScripts[3].m_lasered)
        {
            m_activablePlatforms[3].SetActive(false);
        }
        
       
        
        
    }
}
