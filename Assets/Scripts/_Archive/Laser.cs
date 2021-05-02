using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector3 m_source;
    [SerializeField] Vector3 m_target;
    [SerializeField][Range(0,1)] float m_thickness;
    [SerializeField] LineRenderer m_lr;

    
    private void OnDrawGizmos()
    {
        m_lr = GetComponent<LineRenderer>();
        
        SetLaser(m_source,m_target,m_thickness);
    }

    void Start()
    {
        m_lr = GetComponent<LineRenderer>();
    }

    public void SetLaser(Vector3 p_source, Vector3 p_target, float p_thickness)
    {
        Mathf.Clamp(p_thickness, 0, 1);
        
        m_lr.SetPosition(0,p_source);
        m_lr.SetPosition(1,p_target);

        m_lr.widthMultiplier = p_thickness;
    }
}
