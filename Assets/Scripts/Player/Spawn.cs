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
}
