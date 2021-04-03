using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_sensX;
    [SerializeField] private float m_sensY;

    Camera m_cam;

    float m_mouseX;
    float m_mouseY;

    public float m_multiplier = 0.01f;

    float m_xRotation;
    float m_yRotation;

    private void Start()
    {
        m_cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();

        m_cam.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, m_yRotation, 0);
    }

    void MyInput()
    {
        m_mouseX = Input.GetAxisRaw("Mouse X");
        m_mouseY = Input.GetAxisRaw("Mouse Y");

        m_yRotation += m_mouseX * m_sensX * m_multiplier;
        m_xRotation += m_mouseY * m_sensY * m_multiplier;

        m_xRotation = Mathf.Clamp(m_xRotation, -80f, 80);
    }
}
