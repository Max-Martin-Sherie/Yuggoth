using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PotoManager : MonoBehaviour
{
    //Stock the cylinders gameObjects (children of an empty object) 
    public List<GameObject> m_cylinders = new List<GameObject>();
    //Stock the initial rotations of the cylinders 
    public List<Quaternion> m_initialRotations = new List<Quaternion>();
    //stock the current rotations of the cylinders
   public List<Quaternion> m_currentRotations = new List<Quaternion>();
    //Create the interpolation ratio 
    private float m_interpolationRatio = 1f;
    
    //Create the time for the lerp 
    [SerializeField]  private float m_timeToReset = 2f;
    
    //create the elapsed time 
    [SerializeField]  private float m_elapsedTime;
    
    //bool that check if the tower is reseting 
    [SerializeField] private bool m_reseting;
    
    //cube caster game object 
    private GameObject m_cubeCaster;
    
    //script of the cube caster 
    private CubeCaster m_cubeCasterScript; 

    private void Start()
    {
        //Get the cube caster 
        m_cubeCaster = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        
        //Get the cube caster script 
        m_cubeCasterScript = m_cubeCaster.GetComponent<CubeCaster>();
        
        //Get the cylinders 
        for (int i = 0; i < 3; i++)
        {
            m_cylinders.Add(transform.GetChild (i).gameObject);
        }
        //Get the original rotations of the cylinders
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            m_initialRotations.Add(m_cylinders[i].transform.rotation);
        }
    }

    private void OnEnable()
    {
        CubeCaster.onRaycastComplete += GetCurrentRotation;
    }

    private void OnDisable()
    {
        CubeCaster.onRaycastComplete -= GetCurrentRotation;
    }

    private void GetCurrentRotation()
    {
        if (m_reseting) return;

        m_reseting = true;
        m_elapsedTime = 0;

        for (int i = 0; i < 3; i++) {
            m_currentRotations.Add(m_cylinders[i].transform.rotation);
        }
    }

    private void Update()
    {
        if (m_reseting) GoToInitialRotation();
    }
    
    private void GoToInitialRotation()
    {
        m_elapsedTime += Time.deltaTime;
        float time = m_elapsedTime / m_timeToReset;
        //Start lerp if the raycast is completed to allow cylinder to return to their original pos
        for (int i = 0; i < m_cylinders.Count; i++)
        {
            m_cylinders[i].transform.rotation = Quaternion.Lerp(m_currentRotations[i], m_initialRotations[i],  time);
            
        }

        if (time >= 1f)
        {
            m_currentRotations.Clear();
            m_reseting = false;
            m_cubeCasterScript.m_raycastCompleted = false;
        }
    }
    
}


