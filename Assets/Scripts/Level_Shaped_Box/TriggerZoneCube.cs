using UnityEngine;

    public class TriggerZoneCube : MonoBehaviour
    {
        public CubeKeySO cubeKeyNeeded;
        public delegate void DoorDelegate();
        public static event DoorDelegate onListCompleted;
        private void OnTriggerEnter(Collider p_other)
        {
            if (p_other.gameObject.TryGetComponent(out CubeHolder cubeKeyHolder))
            {
                TriggerZoneContainer triggerZoneContainerScript = GetComponentInParent<TriggerZoneContainer>();
                if (cubeKeyHolder.cubeKeyHolded == cubeKeyNeeded)
                {
                    triggerZoneContainerScript.m_listNeededKeys.Add(cubeKeyHolder.cubeKeyHolded);
                    cubeKeyHolder.cubeKeyHolded = null;
                    if (onListCompleted != null && triggerZoneContainerScript.m_listNeededKeys.Count == triggerZoneContainerScript.m_numberOfKeysNeeded)
                    {
                        onListCompleted();
                    }
                }
            }
        }
    }
