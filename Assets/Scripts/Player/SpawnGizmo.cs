using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGizmo : MonoBehaviour
{
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position,new Vector3(1f,2f,1f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,transform.forward);
    }
}
