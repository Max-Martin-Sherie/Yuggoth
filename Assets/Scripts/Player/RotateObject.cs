using UnityEngine;

/// <summary>
/// This script allows the player to rotate objects on the y axis 
/// </summary>

public class RotateObject : MonoBehaviour
{
    
    [SerializeField][Tooltip("Range at which an object can be rotated")] private float m_range;
    [SerializeField][Tooltip("Speed at which an object can be rotated")] private float m_rotateSpeed;

    // Fetching the scripts that output the player's info to modify
    private CharaControllerRB m_controllerScript;
    private CameraController m_cameraController;
    
    //creating a camera for the raycast
    private Camera m_camera;

    //getting the original movement speed and sensitivity of the player
    private float m_speed;
    private float m_sensitivity;

    private void Start()
    {
        //Fetching the component scripts that output the player's info to modify
        m_controllerScript = GetComponent<CharaControllerRB>();
        m_cameraController = GetComponent<CameraController>();

        //Getting the original movement speed and sensitivity of the player
        m_speed = m_controllerScript.m_moveSpeed;
        m_sensitivity = m_cameraController.m_mouseSensitivity;
        
        //Fetching the main camera for the raycast
        m_camera = Camera.main;
    }

    void Update()
    {
        //Checking if the player has clicked the lmb
        if (Input.GetButton("Fire1"))
        {
            bool target = InteractRaycast.m_hitSomething && InteractRaycast.m_hitTarget.collider.gameObject.layer == LayerMask.NameToLayer("Rotatable");
            //if the player has clicked the lmb and it hit something
            if (target)
            {
                InteractRaycast.m_interacting = true;
                //Fetching the hit object
                GameObject hitObject = InteractRaycast.m_hitTarget.collider.gameObject;
                
                //If the hit object is in the right layer
                if (hitObject.layer == 6)
                {
                    //removing the player's control
                    m_cameraController.m_mouseSensitivity = 0;
                    m_controllerScript.m_moveSpeed = 0;
                    m_controllerScript.m_canJump = false;
                    
                    
                    //Making the player loose all velocity except on the y axis
                    float y = m_controllerScript.m_rb.velocity.y;
                    m_controllerScript.m_rb.velocity = new Vector3(Vector3.zero.x,y,Vector3.zero.z);
                    
                    //Using his mouse laser input to rotate  the object
                    float horizontal = Input.GetAxis("Mouse X");
                    Vector3 rotation = Vector3.up * (m_rotateSpeed * Time.deltaTime * -horizontal);
                    hitObject.transform.Rotate(rotation);
                }
            }
        }
        else
        {
            //If the player isn't pressing the button reseting his speed and sensitivity
            if(m_controllerScript.m_moveSpeed != m_speed)
            {
                m_controllerScript.m_moveSpeed = m_speed;
                m_cameraController.m_mouseSensitivity = m_sensitivity;
                m_controllerScript.m_canJump = true;
                InteractRaycast.m_interacting = false;
                
                Debug.Log("yeah");
            }
        }
    }
}
