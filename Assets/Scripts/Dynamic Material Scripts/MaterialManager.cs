using UnityEngine;

public abstract class MaterialManager : MonoBehaviour
{
    protected Material material;
    protected Renderer objectRenderer;
    protected MaterialPropertyBlock mpb;
    public virtual void OnEnable()
    {
        objectRenderer = GetComponent<Renderer>();
        material = objectRenderer.material;
        mpb = new MaterialPropertyBlock();
    }

    public abstract void UpdateMaterial();
}
