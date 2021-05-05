using System;
using UnityEngine;
using System.Collections.Generic;

public class PotoManager : MonoBehaviour
{
    //Stock the cylinders gameObjects (children of an empty object) 
    public List<GameObject> m_cylinders = new List<GameObject>();
    //Stock the initial rotations of the cylinders 
    public List<Quaternion> m_initialLocalRotations = new List<Quaternion>();
    //stock the current rotations of the cylinders
    public List<Quaternion> m_currentLocalRotations = new List<Quaternion>();
    //Create the interpolation ratio 
    private float m_interpolationRatio = 0.1f;
    


    private void Start()
    {
        //Get the cylinders 
        for (int i = 0; i < 3; i++)
        {
            m_cylinders.Add(transform.GetChild (i).gameObject);
        }
        //Get the original rotations of the cylinders
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            m_initialLocalRotations.Add(transform.localRotation);
        }
        
    }

    private void OnEnable()
    {
        CubeCaster.onRaycastComplete += GetToInitialRotation;
    }

    private void OnDisable()
    {
        CubeCaster.onRaycastComplete -= GetToInitialRotation;
    }

    private void GetToInitialRotation()
    {
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            m_currentLocalRotations.Add(m_cylinders[i].transform.localRotation);
        }
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            //Start lerp if the raycast is completed to allow cylinder to return to their original pos
            Quaternion.Lerp(m_currentLocalRotations[i], m_initialLocalRotations[i], m_interpolationRatio);
        }
        
    }
    
}
