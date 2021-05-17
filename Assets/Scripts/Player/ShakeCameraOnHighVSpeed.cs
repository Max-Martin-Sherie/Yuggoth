using System.Collections;
using UnityEngine;

/// <summary>
/// This class translated the camera on the local x and y axis depending on the y velocity of the player creating a screen shake effect
/// This class requires that the affected gameobject has a Player Move class as well as a camera as child
/// </summary>

public class ShakeCameraOnHighVSpeed : MonoBehaviour
{
    [SerializeField][Tooltip("the maximum translation of the camera in meters")] private float m_maxMagnitude = 0.3f;
    [SerializeField][Tooltip("the minimum vertical speed for the vibration to start")] private float m_minYSpeed = -15f;
    [SerializeField][Tooltip("the rate at which the vibration will accelerate")] private float m_acceleration = 0.06f;
    [SerializeField][Tooltip("the amount of vibrations per second")] private float m_vibrationSpeed = 0.02f;
    
    private Camera m_mainCam;
    
    private PlayerMove m_moveScript;

    private bool m_shaking;
    private Vector3 m_cameraLocalStartPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //Fetching the main camera as well as the character controller
        m_mainCam = GetComponentInChildren<Camera>();
        m_moveScript = GetComponent<PlayerMove>();
        
        //Starting the corountine
        StartCoroutine(ShakeOnYVelocity());
        m_cameraLocalStartPos = m_mainCam.transform.localPosition;
    }

    IEnumerator ShakeOnYVelocity()
    {
        //making sure that the corountine never stops
        while (true)
        {
            //getting the y velocity from the character controller
            float ySpeed = m_moveScript.m_velocity.y;
            
            //If the minimum speed of the player underpasses the minimum required speed for vibration
            if(m_minYSpeed > ySpeed)
            {
                m_shaking = true;
                //Fetching the original position of the camera
                Vector3 localPosition = m_mainCam.transform.localPosition;

                //calculating magnitude of the camera shake with asymptotic progression
                float magnitude = m_maxMagnitude - m_maxMagnitude * Mathf.Exp(m_acceleration * (ySpeed - m_minYSpeed));

                //fetching  a random camera shake
                float x = Random.Range(-magnitude, magnitude);
                float y = Random.Range(-magnitude, magnitude);

                //Affecting the new position of the camera with the shake
                m_mainCam.transform.localPosition = new Vector3(x, y, localPosition.z);
            }
            else if(m_shaking)
            {
                m_shaking = false;
                m_mainCam.transform.localPosition = m_cameraLocalStartPos;
            }
            
            //repeat the function in m_vibrationSpeed seconds 
            yield return new WaitForSeconds(m_vibrationSpeed);
        }
    }
}
