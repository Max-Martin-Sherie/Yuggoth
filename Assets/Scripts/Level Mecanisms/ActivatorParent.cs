using System;
using UnityEngine;

[Serializable]
public class ActivatorParent : MonoBehaviour
{
    /*[HideInInspector]*/public bool m_enabled = false;
    
    //Initiating the delegate that will be called everytime the GameObject gets hit
    public delegate void OnAction();

    //Creating a delegate call
    public OnAction OnActivate;
    
    //Creating a delegate call
    public OnAction OnRelease;
}
