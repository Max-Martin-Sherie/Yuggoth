using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public bool m_lasered = false;
    private Laser[] m_childrenLasers;

    private void Start()
    {
        
        m_childrenLasers = transform.parent.GetComponentsInChildren<Laser>();
    }

    private void Update()
    {
        
        if (m_lasered)
        {
            foreach (Laser script in m_childrenLasers){
                script.m_range = 64;
            }
        }else
        {
            foreach (Laser script in m_childrenLasers)
            {
                script.m_range = 0;
            } 
        }
        
        m_lasered = false;
    }

    public void ActivateChildren()
    {
        m_lasered = true;
    }
}
