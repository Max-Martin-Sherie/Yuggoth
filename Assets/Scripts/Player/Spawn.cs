using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<SpawnPoint> m_spawnPoints;

    [Serializable]
    public struct SpawnPoint
    {
        public String _label;
        public Transform _transform;
    }

    private void Update()
    {
        if (m_spawnPoints.Count >= 1 && Input.GetKeyDown(KeyCode.F1)) SetNewPosition(m_spawnPoints[0]._transform);
        if (m_spawnPoints.Count >= 2 && Input.GetKeyDown(KeyCode.F2)) SetNewPosition(m_spawnPoints[1]._transform);
        if (m_spawnPoints.Count >= 3 && Input.GetKeyDown(KeyCode.F3)) SetNewPosition(m_spawnPoints[2]._transform);
        if (m_spawnPoints.Count >= 4 && Input.GetKeyDown(KeyCode.F4)) SetNewPosition(m_spawnPoints[3]._transform);
        if (m_spawnPoints.Count >= 5 && Input.GetKeyDown(KeyCode.F5)) SetNewPosition(m_spawnPoints[4]._transform);
        if (m_spawnPoints.Count >= 6 && Input.GetKeyDown(KeyCode.F6)) SetNewPosition(m_spawnPoints[5]._transform);
        if (m_spawnPoints.Count >= 7 && Input.GetKeyDown(KeyCode.F7)) SetNewPosition(m_spawnPoints[6]._transform);
        if (m_spawnPoints.Count >= 8 && Input.GetKeyDown(KeyCode.F8)) SetNewPosition(m_spawnPoints[7]._transform);
        if (m_spawnPoints.Count >= 9 && Input.GetKeyDown(KeyCode.F9)) SetNewPosition(m_spawnPoints[8]._transform);
        if (m_spawnPoints.Count >= 10 && Input.GetKeyDown(KeyCode.F10)) SetNewPosition(m_spawnPoints[9]._transform);
        if (m_spawnPoints.Count >= 11 && Input.GetKeyDown(KeyCode.F11)) SetNewPosition(m_spawnPoints[10]._transform);
        if (m_spawnPoints.Count >= 12 && Input.GetKeyDown(KeyCode.F12)) SetNewPosition(m_spawnPoints[11]._transform);
        if (m_spawnPoints.Count >= 13 && Input.GetKeyDown(KeyCode.P)) SetNewPosition(m_spawnPoints[12]._transform);
        if (m_spawnPoints.Count >= 14 && Input.GetKeyDown(KeyCode.M)) SetNewPosition(m_spawnPoints[13]._transform);
    }

    private void SetNewPosition(Transform p_position)
    {
        gameObject.transform.position = p_position.position;
        gameObject.transform.rotation = p_position.rotation;
        
        GetComponent<PlayerMove>().m_velocity = Vector3.zero;
    }
}
