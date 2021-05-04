using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSelector : MonoBehaviour
{
    [SerializeField] private Armor m_armor;
    ArmorStats m_myArmor;
    
    // Start is called before the first frame update
    void Start()
    {
        m_myArmor = new ArmorStats(m_armor.m_name,m_armor.m_jumpHeight, m_armor.m_magnetRange); 
        
        Debug.Log(m_myArmor.m_armorName);
        Debug.Log(m_myArmor.m_jumpHeight);
        Debug.Log(m_myArmor.m_magnetRange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    struct ArmorStats
    {
        public string m_armorName;
        public float m_jumpHeight;
        public float m_magnetRange;
        public ArmorStats(string p_armorName,float p_jumpHeight, float p_magnetRange)
        {
            this.m_armorName = p_armorName;
            this.m_jumpHeight = p_jumpHeight;
            this.m_magnetRange = p_magnetRange;
        }
    }
}
