using UnityEngine;

/// <summary>
/// This script allows the player to rotate objects on the y axis 
/// </summary>

public class RotateObject : MonoBehaviour
{
    [SerializeField][Range(0,1000)][Tooltip("Speed at which an object can be rotated")] private float m_rotateSpeed;

    // Fetching the scripts that output the player's info to modify
    private PlayerMove m_controllerScript;
    private CameraController m_cameraController;

    //getting the original movement speed and sensitivity of the player
    private float m_speed;
    private float m_sensitivity;

    
    private void Start()
    {
        //Fetching the component scripts that output the player's info to modify
        m_controllerScript = GetComponent<PlayerMove>();
        m_cameraController = GetComponent<CameraController>();

        //Getting the original movement speed and sensitivity of the player
        m_speed = m_controllerScript.m_moveSpeed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_sensitivity = CameraController.m_mouseSensitivity;
        }
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
                    CameraController.m_mouseSensitivity = 0;
                    m_controllerScript.m_moveSpeed = 0;
                    m_controllerScript.m_canJump = false;
                    
                    
                    //Making the player loose all velocity except on the y axis
                    float y = m_controllerScript.m_velocity.y;
                    m_controllerScript.m_velocity = new Vector3(0,y,0);
                    
                    //Using his mouse laser input to rotate  the object
                    float horizontal = Input.GetAxis("Mouse X");
                    Vector3 rotation = Vector3.up * (m_rotateSpeed * Time.deltaTime * -horizontal);
                    hitObject.transform.Rotate(rotation);
                }
            }
        }
        
        if(Input.GetButtonUp("Fire1"))
        {
            //If the player isn't pressing the button reseting his speed and sensitivity
            if(m_controllerScript.m_moveSpeed != m_speed)
            {
                m_controllerScript.m_moveSpeed = m_speed;
                CameraController.m_mouseSensitivity = m_sensitivity;
                m_controllerScript.m_canJump = true;
                InteractRaycast.m_interacting = false;
            }
        }
    }
}
