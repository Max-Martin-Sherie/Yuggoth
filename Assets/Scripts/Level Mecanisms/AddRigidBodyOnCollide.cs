using UnityEngine;

public class AddRigidBodyOnCollide : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObject;

    // Update is called once per frame
    void OnTriggerEnter()
    {
        m_gameObject.AddComponent<Rigidbody>();
    }
}
