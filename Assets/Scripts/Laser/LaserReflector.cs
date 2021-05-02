using UnityEngine;

/// <summary>
/// A class that handles the reflection of lasers on the GameObjects collider
/// </summary>

public class LaserReflector : MonoBehaviour
{
    private LaserReceptor m_receptorScript;
    
    
    private LaserReceptor m_hitObjectReceptor = null;
    
    // Start is called before the first frame update
    void Start()
    {
        //Checking if the player has a LaserReceptor script
        bool lrIsPresent = TryGetComponent<LaserReceptor>(out m_receptorScript);
        
        //warning the level designer if he doesn't have a LaserReceptor script
        if (!lrIsPresent)
        {
            Debug.LogWarning($"Hey There is no LineRenderer on {gameObject.name} please add one!");
            gameObject.SetActive(false);
            return;
        }
        
        //Adding the ReflectLaser function to the onReceiveLaser delegate
        m_receptorScript.onReceiveLaser += ReflectLaser;
    }
    
    
    /// <summary>
    /// A function that is called to reflect an incoming laser
    /// </summary>
    /// <param name="p_laserProperties"> the properties of the incoming laser </param>
    void ReflectLaser(LaserReceptor.LaserProperties p_laserProperties)
    {
        //Calculating the direction of the reflected laser
        Vector3 reflectionDirection = Vector3.Reflect(p_laserProperties._incomeDirection,p_laserProperties._hit.normal);

        //Fetching the LineRenderer of the laser source
        LineRenderer lr = p_laserProperties._lineRenderer;
        
        //adding an extra position to the previously fetched line renderer
        lr.positionCount = p_laserProperties._pointCount+1;

        //Defining a source vector position
        Vector3 source = p_laserProperties._hit.point;
        
        //Checking if the reflected laser hits something
        //If the reflected laser hits something...
        if (Physics.Raycast(source, reflectionDirection, out RaycastHit hit, p_laserProperties._range))
        {
            //Setting the position
            lr.SetPosition(p_laserProperties._pointCount,hit.point);

            //Fetching the hit object
            GameObject hitObject = hit.collider.gameObject;

            //Fetching his receptor script and calling the ReceiveLaserFunction
            if (hitObject.TryGetComponent<LaserReceptor>(out LaserReceptor hitObjectReceptor))
            {
                if (m_hitObjectReceptor != hitObjectReceptor)
                {
                    if(m_hitObjectReceptor != null)m_hitObjectReceptor.m_laserHit = false;
                    m_hitObjectReceptor = hitObjectReceptor;
                }

                hitObjectReceptor.ReceiveLaser(p_laserProperties._range,reflectionDirection,p_laserProperties._lineRenderer,hit,p_laserProperties._pointCount+1);
                hitObjectReceptor.m_laserHit = true;
            }
        }
        //... if it doesn't
        else
        {
            lr.SetPosition(p_laserProperties._pointCount,source + reflectionDirection * p_laserProperties._range);
            if (m_hitObjectReceptor && m_hitObjectReceptor.m_laserHit) m_hitObjectReceptor.m_laserHit = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_hitObjectReceptor && !m_receptorScript.m_laserHit)
        {
            m_hitObjectReceptor.m_laserHit = false;
            m_hitObjectReceptor = null;
        }
    }
}
