using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(200, 1000)] public float m_mouseSensitivity = 200;
    [SerializeField] [Range(-1,1)] private int m_yInvertion = -1;
    [SerializeField] [Range(-1, 1)] private int m_xInvertion = 1;

    private Camera m_cam;

    private float m_xRotation;
    private float m_yRotation;

    private void Start()
    {
        m_cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            //Set the values of rotations
            m_yRotation += mouseX * m_mouseSensitivity * m_xInvertion * Time.deltaTime;
            m_xRotation += mouseY * m_mouseSensitivity * m_yInvertion * Time.deltaTime;

            m_xRotation = Mathf.Clamp(m_xRotation, -80f, 80);

            //Affectation of values
            m_cam.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, m_yRotation, 0);
    }

    
}
