using UnityEngine;

public abstract class MaterialManager : MonoBehaviour
{
    protected Material material;
    protected Renderer objectRenderer;
    public virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        material = objectRenderer.material;    
    }
}
