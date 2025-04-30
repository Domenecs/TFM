using NUnit.Framework;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] meshes;
    [SerializeField]
    private LineRenderer[] lines;



    public void ToggleMeshes()
    {
        foreach (var mesh in meshes)
        {
            mesh.enabled = !mesh.enabled;
        }
    }

    public void ToggleLineRenderers()
    {
        foreach (var lineRend in lines)
        {
            lineRend.enabled = !lineRend.enabled;  
        }
    }
}
