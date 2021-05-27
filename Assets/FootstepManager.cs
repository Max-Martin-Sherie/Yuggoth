using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    private PlayerMove m_controller;

    [SerializeField] [Tooltip("AudioSource in charge of footsteps")] private AudioSource m_footStepAudioSource;
    [SerializeField] [Tooltip("List of audio clips of exterior footsteps")] private List<AudioClip> m_footstepsExtList = new List<AudioClip>();
    [SerializeField] [Tooltip("List of audio clips of interior footsteps")] private List<AudioClip> m_footstepsIntList = new List<AudioClip>();
    private bool m_canPlayFootstep;
    
    [HideInInspector] public bool m_isInterior;

    [SerializeField][Tooltip("Interval time between each footstep sound")] [Range(0f, 1f)] private float m_inBetween;
    
    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponentInParent<PlayerMove>();

        m_isInterior = false;
        
        m_canPlayFootstep = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Plays footsteps at regular intervals if the player is moving and if he is on the ground
        if (m_canPlayFootstep == false && m_controller.m_inputMove.magnitude > 0 && m_controller.m_grounded)
        {
            m_canPlayFootstep = true;
            StartCoroutine(FootstepSound());
        }
    }
    
    // FootstepSound is call for playing footsteps
    IEnumerator FootstepSound()
    {
        m_footStepAudioSource.Play();
        yield return new WaitForSeconds(m_inBetween);
        m_canPlayFootstep = false;
    }
}
