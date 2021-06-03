using UnityEngine;

public class ReticuleFeedback : MonoBehaviour
{
    private Animator m_animator;
    [SerializeField][Tooltip("the layer mask of items that the cursor will react to")]private LayerMask m_interactableItems;
    
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool("Active", InteractRaycast.m_hitSomething && m_interactableItems == (m_interactableItems | (1 << InteractRaycast.m_hitTarget.collider.gameObject.layer)));
    }
}
