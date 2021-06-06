using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFix : MonoBehaviour
{

    [SerializeField] private FadeToBlackFromPoint m_fadetoblakcfrompoint;


    private void OnTriggerEnter(Collider other)
    {
        m_fadetoblakcfrompoint.TheEnd();
    }
}
