using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private GameObject m_door;
    private LaserReceptor m_receptorScript;
    [SerializeField] private List<GameObject> m_doors;
    [SerializeField] private List<LaserReceptor> m_laserReceptors;
    
    /// <summary>
    /// Set the game object m_door to be the first door in the list 
    /// </summary>
    private void Start()
    {
        m_door = m_doors[0];
        
        for (int i = 0; i < m_laserReceptors.Count; i++)
        {
            GetComponent<LaserReceptor>();
        }
        
    }

    private void Update()
    {
        for (int i = 0; i < m_doors.Count; i++)
        {
            if (m_laserReceptors[i].m_laserHit)
            {
                m_doors[i].gameObject.SetActive(false);
            }

            else
            {
                m_doors[i].gameObject.SetActive(true);
            }
        }
    }
}
