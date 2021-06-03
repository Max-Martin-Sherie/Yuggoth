using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GravitySimulatorTool : EditorWindow
{
    private bool m_simulating = false;
    private float m_simulationSpeed = 1;

    private List<Rigidbody> m_rbs;
    private List<GameObject> m_selectedObjects;

    private List<Vector3> m_positions;
    private List<Quaternion> m_rotation;
    private List<Vector3> m_scale;
    
    private List<bool> m_hadMeshCollider;
    private List<bool> m_hadRigidBody;


    [MenuItem("Gucci/CashMoney/Gravity Simulator Tool")]
    public static void ShowWindow()
    {
        GetWindow<GravitySimulatorTool>("Gravity Simulator Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Yuggoth c'est le meilleur jeu du monde", EditorStyles.boldLabel);
        GUILayout.Label(" ", EditorStyles.boldLabel);

        m_simulationSpeed = EditorGUILayout.Slider(m_simulationSpeed, 0, 3);

        if (GUILayout.Button("Start simulation"))
        {
            if(!m_simulating){
                m_positions = new List<Vector3>();
                m_rotation = new List<Quaternion>();
                m_scale = new List<Vector3>();

                m_hadMeshCollider = new List<bool>();
                m_hadRigidBody = new List<bool>();

                m_rbs = null;
                m_selectedObjects = null;
                m_rbs = new List<Rigidbody>();

                m_selectedObjects = Selection.gameObjects.ToList();

                foreach (GameObject go in m_selectedObjects)
                {
                    bool hadMeshCollider = go.TryGetComponent(out MeshCollider _);
                    if (!hadMeshCollider)
                    {
                        go.AddComponent<MeshCollider>();
                        go.GetComponent<MeshCollider>().convex = true;
                    }
                    else
                    {
                        go.GetComponent<MeshCollider>().convex = true;
                    }

                    m_hadMeshCollider.Add(hadMeshCollider);

                    bool hadRigidbody = go.TryGetComponent(out Rigidbody _);
                    if (!hadRigidbody)
                    {
                        go.AddComponent<Rigidbody>();
                    }

                    m_hadRigidBody.Add(hadRigidbody);

                    m_positions.Add(go.transform.position);
                    m_rotation.Add(go.transform.rotation);
                    m_scale.Add(go.transform.localScale);
                }

                m_rbs = FindObjectsOfType<Rigidbody>().ToList();

                foreach (Rigidbody rb in m_rbs)
                {
                    if (!rb.isKinematic) rb.isKinematic = true;
                    else m_rbs.Remove(rb);
                }

                foreach (GameObject go in m_selectedObjects)
                {
                    go.GetComponent<Rigidbody>().isKinematic = false;
                }

                Physics.autoSimulation = false;
                m_simulating = true;
            }
        }

        if (GUILayout.Button("Stop simulation"))
        {
            if(m_simulating)
            {
                foreach (Rigidbody rb in m_rbs)
                {
                    rb.isKinematic = false;
                }

                for (int i = 0; i < m_selectedObjects.Count; i++)
                {
                    if (!m_hadMeshCollider[i])
                        DestroyImmediate(m_selectedObjects[i].GetComponent<MeshCollider>());
                    else
                        m_selectedObjects[i].GetComponent<MeshCollider>().convex = false;

                    if (!m_hadRigidBody[i])
                        DestroyImmediate(m_selectedObjects[i].GetComponent<Rigidbody>());

                }

                Physics.autoSimulation = true;
                m_simulating = false;
            }
        }

        GUILayout.Label(" ", EditorStyles.boldLabel);
        GUILayout.Label(" ", EditorStyles.boldLabel);
        GUILayout.Label(" ", EditorStyles.boldLabel);
        GUILayout.Label(" ", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Reset"))
        {
            for (int i = 0; i < m_selectedObjects.Count; i++)
            {
                m_selectedObjects[i].transform.position = m_positions[i];
                m_selectedObjects[i].transform.rotation = m_rotation[i];
                m_selectedObjects[i].transform.localScale = m_scale[i];
            }
        }
        
        GUILayout.Label("Everything is Gucci", EditorStyles.boldLabel);
    }
    
    void Update()
    {
        if(m_simulating)
        {
            //Physics.Simulate(Time.deltaTime * m_simulationSpeed);
            
            Physics.Simulate(0.01f * m_simulationSpeed);
        }
    }

    struct objects
    {
        public Transform _transform;
        public GameObject _object;
    }
}
