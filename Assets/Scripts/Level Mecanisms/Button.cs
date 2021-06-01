using UnityEngine;

/// <summary>
/// Items from this class will act like portal buttons "nuff said"
/// </summary>

public class Button : ActivatorParent
{
    [SerializeField][Tooltip("activator layers")]private LayerMask m_activators;
    [SerializeField][Tooltip("the speed at which the button will be pressed")]private float m_shrinkSpeed = 10f;

    private float m_startWidth; //the original width of the button
    private Vector3 m_bcSize;

    private void Start()
    {
        m_startWidth = transform.localScale.y; //Fetching the original width of the button
        
        m_bcSize = GetComponent<BoxCollider>().size;
    }
    
    private void Update()
    {
        if (Physics.BoxCast(transform.position + Vector3.down * m_bcSize.y / 2, m_bcSize/2, Vector3.up, out RaycastHit hit,transform.rotation,m_bcSize.y*2+0.35f,m_activators))
        {
            //Activating the trigger
            if(!m_enabled)
            {
                m_enabled = true;
                if(OnActivate !=null) OnActivate();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,transform.parent.position + Vector3.down * (m_bcSize.y/1.5f),Time.deltaTime * m_shrinkSpeed);
            }
        }
        else
        {
            //resseting the trigger
            transform.position = transform.parent.position;
            if(m_enabled)
            {
                m_enabled = false;
                if (OnRelease != null)
                {
                    OnRelease();
                }
            }
        }
    }
}