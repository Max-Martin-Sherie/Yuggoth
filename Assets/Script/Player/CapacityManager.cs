using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityManager : MonoBehaviour
{
    public PlayerCompetence[] m_capacityList = new PlayerCompetence[1];

    void Start()
    {
        m_capacityList = GetComponents<PlayerCompetence>();

        for(int i = 0; i < m_capacityList.Length; i++)
        {
            m_capacityList[i].enabled = false;
        }

        Debug.Log(m_capacityList.Length);
    }
}
