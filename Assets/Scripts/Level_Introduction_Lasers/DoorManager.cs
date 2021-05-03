
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private DoorAndReceptors[] m_doorAndReceptors;
    

    private void Update()
    {
        foreach (DoorAndReceptors system in m_doorAndReceptors)
        {
            bool allEnabled = true;
            foreach (LaserReceptor receptor in system._receptors)
            {
                if (!receptor.m_laserHit)
                {
                    allEnabled = false;
                    break;
                }
            }

            if (allEnabled) system._door.SetActive(system._invert);
            else system._door.SetActive(!system._invert);
        }
    }

    [System.Serializable]
    struct DoorAndReceptors
    {
        public GameObject _door;
        public LaserReceptor[] _receptors;
        public bool _invert;
    }
}
