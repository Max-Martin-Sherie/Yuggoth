using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float m_range;
    [SerializeField] private float m_rotateSpeed;

    private CharaControllerRB m_controllerScript;
    private CameraController m_cameraController;
    private Camera m_camera;


    private float m_speed;
    private float m_sensitivity;

    private void Awake()
    {
        m_controllerScript = GetComponent<CharaControllerRB>();
        m_cameraController = GetComponent<CameraController>();

        m_speed = m_controllerScript.m_moveSpeed;
        m_sensitivity = m_cameraController.m_mouseSensitivity;
        m_camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward,out RaycastHit hit, m_range))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    m_controllerScript.m_moveSpeed = 0;
                    m_cameraController.m_mouseSensitivity = 0;
                    float horizontal = Input.GetAxis("Mouse X");
                    Vector3 rotation = Vector3.up * (m_rotateSpeed * Time.deltaTime * -horizontal);
                    hit.collider.gameObject.transform.Rotate(rotation);
                }
            }
        }
        else
        {
            m_controllerScript.m_moveSpeed = m_speed;
            m_cameraController.m_mouseSensitivity = m_sensitivity;
        }
    }
}
