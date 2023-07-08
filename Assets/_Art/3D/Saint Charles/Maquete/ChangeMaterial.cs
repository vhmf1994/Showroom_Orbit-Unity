using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<MeshRenderer> meshes;

    [SerializeField] private Material material;

    private void OnValidate()
    {
        if (Application.isPlaying) return;

        meshes = GetComponentsInChildren<MeshRenderer>().ToList();

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = material;
            mesh.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            mesh.allowOcclusionWhenDynamic = false;
        }
    }
}
