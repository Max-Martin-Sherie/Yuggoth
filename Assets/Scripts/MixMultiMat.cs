using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MixMultiMat : MonoBehaviour
{
    [SerializeField]private Material[] m_materials;
    [SerializeField]private bool m_moveDuringPlay;
    [SerializeField]private float m_timeBetweenSwitches = 1f;

    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        if (m_moveDuringPlay) StartCoroutine(CallFunction());
    }

    IEnumerator CallFunction()
    {
        while (m_moveDuringPlay)
        {
            SetRandomMaterials();
            yield return new WaitForSeconds(m_timeBetweenSwitches);
        }
    }
    
    [ContextMenu("Set Random Materials")]
    public void SetRandomMaterials()
    {
        if (!TryGetComponent(out m_meshRenderer))
        {
            Debug.LogWarning("No MeshRenderer");
        }

        MeshRenderer newMR = new MeshRenderer();

        List<Material> mats = new List<Material>();

        // foreach (Material mat in m_meshRenderer.materials)
        //     mats.Add(new Material(m_materials[Random.Range(0, m_materials.Length)]));

        for (int i = 0; i < m_meshRenderer.materials.Length; i++)
        {
            if (i == 0)
                mats.Add(m_meshRenderer.materials[0]);
            else
                mats.Add(new Material(m_materials[Random.Range(0, m_materials.Length)]));
        }
        
        m_meshRenderer.materials = mats.ToArray();
    }
}
