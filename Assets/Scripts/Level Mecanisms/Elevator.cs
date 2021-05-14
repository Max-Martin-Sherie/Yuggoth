using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float m_targettedHeight;
    [SerializeField] private float m_moveSpeed = 10f;

    
     private float m_startHeight;
    
    private bool m_movingDown = false;
    public bool m_movingUp = false;

    private SphereCollider m_sc;
    private BoxCollider m_bc;

    [SerializeField]private GameObject m_player;
    
    // Start is called before the first frame update
    void Start()
    {
        m_sc = GetComponent<SphereCollider>();
        m_bc = GetComponent<BoxCollider>();

        m_startHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_movingDown)
        {
            Vector3 targetPosition = new Vector3(transform.localPosition.x, m_targettedHeight, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition,targetPosition, m_moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_movingDown = true;
            m_sc.enabled = false;
        }
    }
}
