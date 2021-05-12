using UnityEngine;

/// <summary>
/// This class will pull the player towards it faster and faster removing the player's control over it slowly.
/// This creates a creepy effect of tension
/// </summary>

public class Pull_Player_To_Point : MonoBehaviour
{
    [SerializeField][Tooltip("the player's control script")] private PlayerMove m_playerMoveScript;
    [SerializeField][Tooltip("the strength at which the player will start being pulled")] private float m_basePullStrength = 5f;
    [SerializeField][Tooltip("the strength that will added to the pull strength per second")] private float m_pullStrengthAcceleration = 4f;
    [SerializeField][Tooltip("the maximum distance from the player at which the class will destroy itslef")] private float m_targetMinDistance = 2f;
    [SerializeField][Tooltip("the speed multiplier of the player that will slow him down (keep above 0.9 for good effect)")] [Range(0,1)]private float m_playerSpeedMutiplier = 0.95f;

    private float m_speed; // the base speed of the player
    private bool m_targeted = false; // checking if the player has been target or not
    
    private void OnDrawGizmos()
    {
        //Drawing the collision sphere for the user
        Gizmos.DrawWireSphere(transform.position, m_targetMinDistance);
    }

    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        m_speed = m_playerMoveScript.m_moveSpeed;
    }

    /// <summary>
    /// OnTriggerEnter is called once when the player triggers the game collider
    /// </summary>
    /// <param name="p_playerCollider"> the collider </param>
    private void OnTriggerEnter(Collider p_playerCollider)
    {
        if (p_playerCollider.gameObject.layer == LayerMask.NameToLayer("Player")) //checking if the trigger object is the player
        {
            m_targeted = true; //setting targeted to true
            m_playerMoveScript.m_canJump = false; //stopping the player from jumping
            Destroy(gameObject.GetComponent<Collider>()); //removing the collider that wont be useful anymore
        }
    }
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (m_targeted) //checking if the player is being targeted 
        {
            //calculating a normalized direction towards the player
            Vector3 direction = Vector3.Normalize(transform.position - m_playerMoveScript.gameObject.transform.position);
            
            //adding an acceleration to the player velocity
            m_playerMoveScript.m_velocity += direction * (Time.deltaTime * m_basePullStrength);
            
            //adding to the pull strength
            m_basePullStrength += m_pullStrengthAcceleration * Time.deltaTime;
            
            //Removing the player's control when he isn't grounded
            if(!m_playerMoveScript.m_grounded)m_playerMoveScript.m_moveSpeed = 0;
            
            else m_playerMoveScript.m_moveSpeed *= m_playerSpeedMutiplier; //slowing down the player's speed
            
            //if the player is within distance : reset everything
            if (Vector3.Distance(m_playerMoveScript.gameObject.transform.position, transform.position) <= m_targetMinDistance)ExitPull();
        }
    }

    /// <summary>
    /// This function will reset the player's modified variable to its default state
    /// </summary>
    private void ExitPull()
    {
        m_playerMoveScript.m_moveSpeed = m_speed; //reset speed
        m_playerMoveScript.m_canJump = true; //reset speed
        Destroy(this.gameObject); //reset speed
    }
}
