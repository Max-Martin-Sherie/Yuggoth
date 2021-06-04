using UnityEngine;

public class FollowTargetedPlatform : MonoBehaviour
{
    private float m_speed = 8;

    [HideInInspector]public Transform m_target;
    [HideInInspector]public ActivatorParent m_ActivatorParent;

    private void Start()
    {
        transform.position += Vector3.up * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,m_target.position) < 0.2f)
            Destroy(gameObject,2f);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime * m_speed);
    }
}
