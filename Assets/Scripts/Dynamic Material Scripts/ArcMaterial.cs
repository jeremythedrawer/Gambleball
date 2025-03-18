using UnityEngine;

public class ArcMaterial : MaterialManager
{
    public static ArcMaterial Instance { get; private set; }
    private Ball activeBall;

    private float width;
    private float height;

    private Vector2 topRight;

    public static bool tutorialMode { get; set; } = true;
    private static readonly int tutorialModeID = Shader.PropertyToID("_TutorialMode");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateMaterial();

        activeBall = LevelManager.Instance.activeBall;
        transform.position = activeBall.transform.position;

        if (Input.GetMouseButtonDown(0) || 
        activeBall.rigidBodyBall.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            topRight = GameManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (activeBall.rigidBodyBall.linearVelocity.y <= 0)
        {
            topRight = activeBall.transform.position;
        }
        width = topRight.x - transform.position.x;
        height = Mathf.Max(topRight.y - transform.position.y, 0);

        transform.localScale = new Vector2( width, height );
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetInt(tutorialModeID, tutorialMode ? 1 : 0);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
