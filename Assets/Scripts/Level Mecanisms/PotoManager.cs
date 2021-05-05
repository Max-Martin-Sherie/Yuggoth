using System;
using UnityEngine;
using System.Collections.Generic;

public class PotoManager : MonoBehaviour
{
    //Stock the cylinders gameObjects 
    public List<GameObject> m_cylinders = new List<GameObject>();
    //Stock the original rotations of the cylinders 
    public List<Quaternion> m_initialLocalRotations = new List<Quaternion>();

    //Vector3.Lerp(Vector3.up, Vector3.forward, interpolationRatio);

    private void Start()
    {
        //Get the original rotations of the cylinders
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            m_initialLocalRotations.Add(transform.localRotation);
        }
        
    }
}
