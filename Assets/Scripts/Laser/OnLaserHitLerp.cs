using System;
using UnityEngine;

public class OnLaserHitLerp : MonoBehaviour
{
      
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    private LaserReceptor m_receptorScript;

    //Creating the variable that will contain the Material of the gameObject
    [SerializeField]private MovingPlatform[] m_gameObjectsToActivate;
    
    //Creating the variable that will contain the initial color of the game object 
    private Color m_initialColor; 

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
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

        for (int i = 0; i < m_gameObjectsToActivate.Length; i++)
        {
            m_gameObjectsToActivate[i]._ogPos = m_gameObjectsToActivate[i]._obj.transform.position;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (m_receptorScript.m_laserHit)
            foreach (MovingPlatform platform in m_gameObjectsToActivate)
            {
                Vector3 position = platform._obj.transform.position;
                platform._obj.transform.position = Vector3.Lerp(position,platform._targetPos,platform._moveSpeed * Time.deltaTime);
            }
        else
            foreach (MovingPlatform platform in m_gameObjectsToActivate)
            {
                Vector3 position = platform._obj.transform.position;
                platform._obj.transform.position = Vector3.Lerp(position,platform._ogPos,platform._moveSpeed * Time.deltaTime);
            }
    }
    
    [Serializable]
    public struct MovingPlatform
    {
        public GameObject _obj;
        public Vector3 _targetPos;
        public float _moveSpeed;
        [HideInInspector]public Vector3 _ogPos;
    }
}
