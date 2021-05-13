using UnityEngine;

public class CameraSway : MonoBehaviour
{
    private float m_timer = 0f;

    [SerializeField] private float m_swaySpeed = 1f;
    [SerializeField] private float m_swayMagnitude = 1f;
    [SerializeField] private Camera m_camera;
    private PlayerMove m_controller;
    
    private Vector3 m_cameraLocalStartPos;
    
    public bool m_swaying = false;
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_controller = GetComponent<PlayerMove>();
        
        m_cameraLocalStartPos = m_camera.transform.localPosition;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (m_swaying && m_controller.m_grounded)
        {
            Vector3 speed = m_controller.m_velocity;

            Debug.Log(speed.magnitude / m_controller.m_moveSpeed / Time.deltaTime);
            
            float newYVelocity = Mathf.Sin(m_timer * m_swaySpeed * Mathf.PI) * m_swayMagnitude * speed.magnitude / m_controller.m_moveSpeed;
            
            m_camera.transform.localPosition = m_cameraLocalStartPos + new Vector3(m_cameraLocalStartPos.x, newYVelocity,m_cameraLocalStartPos.z);
            m_timer += Time.deltaTime;
        }
        else
        {
            m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, m_cameraLocalStartPos,5f * Time.deltaTime);
            m_timer = 0;
        }
    }
}
