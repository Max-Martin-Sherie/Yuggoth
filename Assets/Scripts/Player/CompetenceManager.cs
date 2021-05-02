using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetenceManager : MonoBehaviour
{
    //Tableau de script de compétences
    public PlayerCompetence[] m_capacityList = new PlayerCompetence[1];

    void Start()
    {
        //Récupération des scripts enfants de PlayerCompetence
        m_capacityList = GetComponents<PlayerCompetence>();

        for(int i = 0; i < m_capacityList.Length; i++)
        {
            m_capacityList[i].enabled = false;
        }

        Debug.Log(m_capacityList.Length);
    }
}
