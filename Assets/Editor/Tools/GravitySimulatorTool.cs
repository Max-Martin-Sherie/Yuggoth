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
        
        if (GUILayout.Button("Toggle simulation"))
        {
            if(!m_simulating)
            {
                m_positions = new List<Vector3>();
                m_rotation = new List<Quaternion>();
                m_scale = new List<Vector3>();
                
                m_rbs = null;
                m_selectedObjects = null;
                m_rbs = new List<Rigidbody>();

                m_selectedObjects = Selection.gameObjects.ToList();
                
                foreach (GameObject go in m_selectedObjects)
                {
                    if (!go.TryGetComponent(out MeshCollider _))
                    {
                        go.AddComponent<MeshCollider>();
                        go.GetComponent<MeshCollider>().convex = true;
                    }

                    if (!go.TryGetComponent(out Rigidbody _))
                    {
                        go.AddComponent<Rigidbody>();
                    }

                    m_positions.Add(go.transform.position);
                    m_rotation.Add(go.transform.rotation);
                    m_scale.Add(go.transform.localScale);
                }

                m_simulating = true;
                Physics.autoSimulation = false;
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
            }
            else
            {
                m_simulating = false;
                Physics.autoSimulation = true;

                foreach (Rigidbody rb in m_rbs)
                {
                    rb.isKinematic = false;
                }
                foreach (GameObject go in m_selectedObjects)
                {
                    if(go.TryGetComponent(out MeshCollider _))
                    {
                        DestroyImmediate(go.GetComponent<MeshCollider>());
                    }
                    if (go.TryGetComponent(out Rigidbody _))
                    {
                        DestroyImmediate(go.GetComponent<Rigidbody>());
                    }
                }
            }
        }

        if (GUILayout.Button("Reset"))
        {
            for (int i = 0; i < m_selectedObjects.Count; i++)
            {
                m_selectedObjects[i].transform.position = m_positions[i];
                m_selectedObjects[i].transform.rotation = m_rotation[i];
                m_selectedObjects[i].transform.localScale = m_scale[i];
            }
        }
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
