using System;
using System.Collections.Generic;
using UnityEngine;

public class OnActivateLerp : MonoBehaviour
{
      
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    private ActivatorParent m_activatorScript;

    [SerializeField] private bool m_multipleParents = false;
    
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    [SerializeField]private ActivatorParent[] m_activatorScripts;

    //Creating the variable that will contain the Material of the gameObject
    [SerializeField]private MovingPlatform[] m_gameObjectsToLerp;

    [SerializeField] private GameObject m_particuleSystemPrefab;

    private bool m_onTarget = true;
    
    public List<Vector3> m_positions;

    private void OnDrawGizmosSelected()
     {
         
         Gizmos.color = Color.yellow;
         foreach (MovingPlatform obj in m_gameObjectsToLerp)
         {
             if(obj._obj)
             {
                 BoxCollider bc;
                 if (obj._obj.TryGetComponent(out bc))
                 {
                     Gizmos.DrawWireCube(obj._targetPos + obj._obj.transform.rotation * bc.center,
                         obj._obj.transform.rotation * bc.size);
                 }
                 else
                 {
                     Gizmos.DrawLine(obj._targetPos - 0.1f * Vector3.up, obj._targetPos + 0.1f * Vector3.up);
                     Gizmos.DrawLine(obj._targetPos - 0.1f * Vector3.up, obj._targetPos + 0.1f * Vector3.left);
                     Gizmos.DrawLine(obj._targetPos - 0.1f * Vector3.up, obj._targetPos + 0.1f * Vector3.forward);
                 }
                 
                 Gizmos.DrawLine(obj._targetPos,obj._obj.transform.position);
             }
         }
    
         m_positions.Clear();
         
         for (int i = 0; i < m_gameObjectsToLerp.Length; i++)
         {
             if(m_gameObjectsToLerp[i]._obj != null)m_positions.Insert(i,m_gameObjectsToLerp[i]._obj.transform.position);
             else
             {
                 Debug.LogWarning($"there is a movable platform that is null on : {gameObject.name} on index {i}");
                 Gizmos.color = Color.red;
                 Gizmos.DrawRay(transform.position,Vector3.up * 10000f);
             }
         }
     }

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        if(!m_multipleParents){
            //Checking if the player has a LaserReceptor script
            bool activatorScript = TryGetComponent(out m_activatorScript);

            //warning the level designer if he doesn't have a LaserReceptor script
            if (!activatorScript)
            {
                Debug.LogWarning($"Hey There is no Activator on {gameObject.name} please add one!");
                gameObject.SetActive(false);
                //return;
            }
        }
        foreach (MovingPlatform obj in m_gameObjectsToLerp)
        {
            obj.SetOgPos();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (m_multipleParents)
        {
            foreach (ActivatorParent activator in m_activatorScripts)
            {
                if (activator.m_enabled)
                {
                    foreach (MovingPlatform platform in m_gameObjectsToLerp)
                        platform.LerpToTarget();
                    return;
                }
            }

            foreach (ActivatorParent activator in m_activatorScripts)
            {
                foreach (MovingPlatform platform in m_gameObjectsToLerp)
                    platform.LerpBack();
            }
            
            
            return;
        }
        if (m_activatorScript.m_enabled)
        {
            if (m_onTarget)
            {
                foreach (MovingPlatform platform in m_gameObjectsToLerp)
                {
                    GameObject obj = Instantiate(m_particuleSystemPrefab);
                    obj.transform.position = transform.position;
                    obj.GetComponent<FollowTargetedPlatform>().m_target = platform._obj.transform;
                    obj.GetComponent<FollowTargetedPlatform>().m_ActivatorParent = m_activatorScript;
                }

                m_onTarget = false;
            }
            foreach (MovingPlatform platform in m_gameObjectsToLerp)
                platform.LerpToTarget();
        }
        else
        {
            foreach (MovingPlatform platform in m_gameObjectsToLerp)
                platform.LerpBack();
            
            m_onTarget = true;
        }
    }
    
    [Serializable]
    public class MovingPlatform
    {
        private Vector3 _originalPosition = Vector3.zero;
        public GameObject _obj;
        public Vector3 _targetPos;
        public float _moveSpeed;

        public void SetOgPos()
        {
            _originalPosition = _obj.transform.position;
        }
        
        public void LerpToTarget()
        {
            _obj.transform.position = Vector3.Lerp(_obj.transform.position,_targetPos,_moveSpeed * Time.deltaTime);
        }
        public void LerpBack()
        {
            _obj.transform.position = Vector3.Lerp(_obj.transform.position,_originalPosition,_moveSpeed * Time.deltaTime);
        }
    }
}
