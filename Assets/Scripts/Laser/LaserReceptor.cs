using UnityEngine;

/// <summary>
/// This class is the backbone of the laser reactivity system.
/// It detects when the GameObject is hit by a laser and collects useful data from it
/// </summary>

public class LaserReceptor : MonoBehaviour
{
    [HideInInspector]
    public bool m_laserHit = false;
    
    //Declaration of the variable that will hold all the info about the incoming laser
    private LaserProperties m_receivedLaser;

    //Initiating the delagate that will be called everytime the GameObject gets hit
    public delegate void OnReceiveLaser(LaserProperties receivedLaser);

    //Creating a delacate call
    public OnReceiveLaser onReceiveLaser;
    
    /// <summary>
    /// This function will be called by laser sources when they hit the GameObject
    /// </summary>
    /// <param name="p_range"> the range of the laser </param>
    /// <param name="m_incomeDirection"> the direction of the incoming </param>
    /// <param name="p_lineRenderer"> The source line renderer </param>
    /// <param name="p_hit"> the hit information </param>
    /// <param name="p_pointCount"> the indes of the position in the line renderer at which the laser hit the GameObject </param>
    public void ReceiveLaser(float p_range,Vector3 p_incomeDirection, LineRenderer p_lineRenderer, RaycastHit p_hit, int p_pointCount)
    {
        //Affecting the parameters onto the laser properties
        m_receivedLaser = new LaserProperties(p_range,p_incomeDirection,p_lineRenderer,p_hit,p_pointCount);

        //Calling the delegate
        if(onReceiveLaser != null)onReceiveLaser(m_receivedLaser);
    }

    //The laser properties structure
    public struct LaserProperties
    {
        public float _range;
        public Vector3 _incomeDirection;
        public LineRenderer _lineRenderer;
        public RaycastHit _hit;
        public int _pointCount;
        
        //Constructor
        public LaserProperties(float p_range, Vector3 p_incomeDirection, LineRenderer p_lineRenderer, RaycastHit p_hit, int p_pointCount)
        {
            _range = p_range;
            _incomeDirection = p_incomeDirection;
            _lineRenderer = p_lineRenderer;
            _hit = p_hit;
            _pointCount = p_pointCount;
        }
    }
}
