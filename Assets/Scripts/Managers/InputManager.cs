using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public bool leftMouseButtonDown => Input.GetMouseButtonDown(0);

    public bool resetInput => Input.GetKeyDown(KeyCode.R);
    public bool nextLevelInput => Input.GetKeyUp(KeyCode.RightArrow);
    public bool quitInput => Input.GetKeyDown(KeyCode.Q);

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}
