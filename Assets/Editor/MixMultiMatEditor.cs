using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MixMultiMat))]
public class MixMultiMatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        MixMultiMat myTarget = (MixMultiMat)target;

        if (GUILayout.Button("Mix!"))
        {
            myTarget.SetRandomMaterials();
        }
    }

}
