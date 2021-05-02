using UnityEngine;

public class InteractRaycast : MonoBehaviour
{
    static public bool m_interacting = false;
    static public bool m_hitSomething = false;

    private Camera m_camera;
    
    [SerializeField][Range(0,30)] float m_rangeSetter = 20f;

    public static float m_range;

    
    static public RaycastHit m_hitTarget;

    private void Start()
    {
        m_camera = Camera.main;
        m_range = m_rangeSetter;
    }

    private void Update()
    {
        if (!m_interacting)
        {
            m_hitSomething = Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out m_hitTarget, m_range);
        }
    }
}
