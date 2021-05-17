using UnityEngine;

/// <summary>
/// This adds camera sway to the player's movement
/// </summary>

public class CameraSway : MonoBehaviour
{
    private float m_timer = 0f; //the timer that will be used to calculate the sway

    [SerializeField][Tooltip("The speed at which the camera will complete a half oscillation")][Range(0,5)] private float m_swaySpeed = 1f;
    [SerializeField][Tooltip("The maximum value of an oscillation")][Range(0,5)] private float m_swayMagnitude = 1f;
    [SerializeField][Tooltip("The player's camera")] private Camera m_camera;
    [SerializeField][Tooltip("The player sway")]private bool m_swaying;
    
    private PlayerMove m_controller; //Player controller
    
    private Vector3 m_cameraLocalStartPos; //the camera's start position
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_controller = GetComponent<PlayerMove>(); //Fetching the controller script
        
        m_cameraLocalStartPos = m_camera.transform.localPosition; //Affecting the original position
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //checking if the player is grounded and allowed to sway
        if (m_swaying && m_controller.m_grounded)
        {
            Vector3 speed = m_controller.m_velocity; //Getting the player's speed
            if(m_controller.m_moveSpeed != 0){
                //Making the new velocity of the timer
                //https://www.geogebra.org/calculator/cj65ygrp
                float newYVelocity = Mathf.Sin(m_timer * m_swaySpeed * Mathf.PI) * m_swayMagnitude * speed.magnitude / m_controller.m_moveSpeed;
                
                //Affecting the new y position to the camera
                m_camera.transform.localPosition = m_cameraLocalStartPos + new Vector3(m_cameraLocalStartPos.x, newYVelocity,m_cameraLocalStartPos.z);
                m_timer += Time.deltaTime; // adding time to the timer
            }
        }
        else
        {
            //reseting the camera position
            m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, m_cameraLocalStartPos,5f * Time.deltaTime);
            
            //Reseting the timer
            m_timer = 0;
        }
    }
}
