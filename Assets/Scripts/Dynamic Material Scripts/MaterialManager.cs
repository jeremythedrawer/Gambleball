using UnityEngine;

public abstract class MaterialManager : MonoBehaviour
{
    protected Material material;

    public virtual void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        material = renderer.material;    
    }
}
