using UnityEngine;

public class TimerTower : MonoBehaviour
{
    //VARIBLES 
    //initial coordinates of the objet 
    private Vector3 m_initialPos;
    
    //locked position of the object 
    private Vector3 m_lockPos; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Get the initial position of the object 
        m_initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Creating a lerp into a coroutine that will allow the object to get back to its initial position (timer) 
    /// </summary>
    private void SlowlyReturnToInitialPos()
    {
        
    }
}
