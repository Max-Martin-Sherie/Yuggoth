using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetenceActivator : MonoBehaviour
{
    [SerializeField] [Range(0,10)] private int m_index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CompetenceManager>().m_capacityList[m_index].enabled = true;
            Destroy(gameObject);
        }
    }
}
