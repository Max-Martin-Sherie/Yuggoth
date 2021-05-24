using System;
using UnityEngine;

public class PontLevis : MonoBehaviour
{
    [SerializeField][Tooltip("Drag and drop a gameobject that has button component here")] private Button m_button;

    [SerializeField][Tooltip("The speed at which the bridge will lower")] private float m_loweringSpeed = 5f;
    [SerializeField][Tooltip("The speed at which the bridge will lower")] private Vector3 m_targetRotation;

    Quaternion m_ogRotation;

    private void Start()
    {
        m_ogRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(((ActivatorParent) m_button).m_enabled)transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(m_targetRotation),m_loweringSpeed * Time.deltaTime );
        else transform.rotation = Quaternion.Lerp(transform.rotation,m_ogRotation,m_loweringSpeed * Time.deltaTime );
    }
}
