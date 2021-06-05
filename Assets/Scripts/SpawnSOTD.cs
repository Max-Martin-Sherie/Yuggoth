using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSOTD : MonoBehaviour
{
    [SerializeField] private GameObject m_sotdPrefab;
    [SerializeField] private Vector2 m_spawnRate;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position,12.5f);
    }

    private void Start()
    {
        StartCoroutine(SpawnSpirits());
    }

    IEnumerator SpawnSpirits()
    {
        yield return new WaitForSeconds(Random.Range(0, m_spawnRate.x));

        while (true)
        {
            Instantiate(m_sotdPrefab, transform.position, m_sotdPrefab.transform.rotation);
            yield return new WaitForSeconds(Random.Range(m_spawnRate.x, m_spawnRate.y));
        }
    }
}
