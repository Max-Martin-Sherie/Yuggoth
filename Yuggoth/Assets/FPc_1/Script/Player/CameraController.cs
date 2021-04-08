using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_mouseSensitivity = 100;

    private Camera m_cam;
    private OxygeneTimer m_oxygeneTimer;

    private float m_xRotation;
    private float m_yRotation;

    private void Start()
    {
        m_cam = GetComponentInChildren<Camera>();
        m_oxygeneTimer = GetComponent<OxygeneTimer>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!m_oxygeneTimer.m_stopTimer)
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            //Set the values of rotations
            m_yRotation += mouseX * m_mouseSensitivity * Time.deltaTime;
            m_xRotation += -mouseY * m_mouseSensitivity * Time.deltaTime;

            m_xRotation = Mathf.Clamp(m_xRotation, -80f, 80);

            //Affectation of values
            m_cam.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, m_yRotation, 0);
        }
    }

}
