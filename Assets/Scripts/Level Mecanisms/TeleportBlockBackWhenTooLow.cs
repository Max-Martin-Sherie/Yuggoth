using UnityEngine;

public class TeleportBlockBackWhenTooLow : MonoBehaviour
{
    [SerializeField] private float m_yDistance = -100;

    private Vector3 m_ogPos;
    private Quaternion m_ogRot;
    
    // Start is called before the first frame update
    void Start()
    {
        m_ogPos = transform.position;
        m_ogRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= m_yDistance)
        {
            transform.position = m_ogPos;
            transform.rotation = m_ogRot;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
