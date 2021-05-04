using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enable_RB_On_Trigger : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rb;

    // Update is called once per frame
    private void OnTriggerEnter(Collider p_other)
    {
        if(p_other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_rb.useGravity = true;
            Destroy(this);
        }
    }
}
