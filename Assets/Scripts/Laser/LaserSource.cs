using UnityEngine;

/// <summary>
/// This class send out a laser from the front of the object.
/// Note : it is necessary that this object has a line renderer
/// </summary>

public class LaserSource : MonoBehaviour
{
    //The thickness of the maser
    [SerializeField]
    [Tooltip("Range in meters")] private float m_laserRange = 10;
    
    //The light renderer
    private LineRenderer m_lr;

    //The hit object receptor script
    private LaserReceptor m_hitObjectReceptor = null;
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Awake()
    {
        //Checking if the player has a line renderer
        bool lrIsPresent = TryGetComponent<LineRenderer>(out m_lr);
        
        //warning for the 50iq level designer if he doesn't have a line renderer
        if (!lrIsPresent)
        {
            Debug.LogWarning($"Hey There is no LineRenderer on {gameObject.name} please add one!");
            gameObject.SetActive(false);
            return;
        }
        
        //Reseting the line renderer (just in case)
        m_lr.positionCount = 2;
    }
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //Setting the laser's position correctly
        m_lr.SetPosition(0, transform.position);
        
        //Doing a raycast to simulate if the laser hit something or not
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_laserRange))
        {
            //Seeting the position of the second point the first shot
            m_lr.SetPosition(1,hit.point);
            
            //Fetching the hit object
            GameObject hitObject = hit.collider.gameObject;

            //Fetching his receptor script and calling the ReceiveLaserFunction
            if (hitObject.TryGetComponent<LaserReceptor>(out LaserReceptor hitObjectReceptor))
            {
                //Making sure that the m_laserHit boolean stays loyal yo the simulation
                if (m_hitObjectReceptor != hitObjectReceptor)
                {
                    if(m_hitObjectReceptor != null)m_hitObjectReceptor.m_laserHit = false;
                    m_hitObjectReceptor = hitObjectReceptor;
                }

                m_lr.positionCount = 2;
                
                hitObjectReceptor.ReceiveLaser(m_laserRange,transform.forward,m_lr,hit,2);
                hitObjectReceptor.m_laserHit = true;
            }
            
        }
        else
        {
            //Reseting the Line renderer 
            Vector3 endPosition = transform.position + (transform.forward * m_laserRange);
            if(m_lr.GetPosition(1)!= endPosition)m_lr.SetPosition(1,endPosition);

            m_lr.positionCount = 2;

            if (m_hitObjectReceptor)
            {
                m_hitObjectReceptor.m_laserHit = false;
                m_hitObjectReceptor = null;
            }
        }
    }

    //Resetting the Line Renderer on disable and enable to avoid issues
    private void OnDisable()
    {
        m_lr.positionCount = 0;
    }

    private void OnEnable()
    {
        m_lr.positionCount = 2;
    }
}