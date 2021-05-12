using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Spawn))]
public class SpawnEditor : Editor
{
    private Spawn m_target;

    private void OnEnable() {
        m_target = target as Spawn;
    }

    public override void OnInspectorGUI()
    {
        Rect r = (Rect)EditorGUILayout.BeginVertical("Button");
        r.height = 16f;

        DrawDropdown(r, new GUIContent("Spawn Point"));

        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(25f);
        
        base.OnInspectorGUI();
    }
    
    
    void DrawDropdown(Rect position, GUIContent label)
    {
        
        if (!EditorGUI.DropdownButton(position, label, FocusType.Passive)) {
            return;
        }
 
        GenericMenu menu = new GenericMenu();

        foreach (Spawn.SpawnPoint spawnPoint in m_target.m_spawnPoints) {
            menu.AddItem(new GUIContent(spawnPoint._label), false, HandleItemClicked, spawnPoint);
        }
   
        menu.DropDown(position);
    }
    
    void HandleItemClicked(object p_parameter) {
        
        Spawn.SpawnPoint spawnPoint =(Spawn.SpawnPoint) p_parameter;

        m_target.gameObject.transform.position = spawnPoint._transform.position;
        m_target.gameObject.transform.rotation = spawnPoint._transform.rotation;
    }
}
