using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneContainer : MonoBehaviour
{
    public List<CubeKeySO> m_listNeededKeys = new List<CubeKeySO>();
    public int m_numberOfKeysNeeded;
    private GameObject m_door;

    [SerializeField] private List<GameObject> m_doors;
    private void Start()
    {
        m_door = m_doors[0];
    }

    private void OnEnable()
    {
        TriggerZoneCube.onListCompleted += OpenDoor;
    }
    
    private void OnDisable()
    {
        TriggerZoneCube.onListCompleted -= OpenDoor;
    }

    private void OpenDoor()
    {
        //m_door? -> verifie si m_door != null 
        m_door?.gameObject.SetActive(false);
        NextDoor();
        
    }

    private void NextDoor()
    {
        if (m_doors != null && m_doors.Count > 0)
        {
            m_doors.RemoveAt(0);
            m_door = m_doors[0];
            return;
        }

        m_door = null;
    }
}
