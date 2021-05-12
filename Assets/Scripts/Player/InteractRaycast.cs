using UnityEngine;

public class InteractRaycast : MonoBehaviour
{
    static public bool m_interacting = false;
    static public bool m_hitSomething = false;

    private Camera m_camera;
    
    [SerializeField][Range(0,30)] float m_rangeSetter = 20f;

    public static float m_range;

    [SerializeField] private LayerMask m_layerMask;
    static public RaycastHit m_hitTarget;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out RaycastHit hit, m_range, m_layerMask))
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance);
        else
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * m_range);
    }
    
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
