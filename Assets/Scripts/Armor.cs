using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "ScriptableObjects/Player/Armor")]
public class Armor : ScriptableObject
{
    public string m_name;
    public float m_jumpHeight;
    public float m_magnetRange;

}
