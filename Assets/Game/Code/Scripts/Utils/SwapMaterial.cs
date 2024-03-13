using UnityEngine;

public class SwapTexture : MonoBehaviour
{
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _swapMaterial;
    
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [ContextMenu("SetBaseMaterial")]
    public void SetBaseMaterial() => _meshRenderer.materials = new[] { _meshRenderer.materials[0], _baseMaterial };
    
    [ContextMenu("SetSwapMaterial")]
    public void SetSwapMaterial() => _meshRenderer.materials = new[] { _meshRenderer.materials[0], _swapMaterial };
}