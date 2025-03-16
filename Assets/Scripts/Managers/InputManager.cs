using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public bool leftMouseButtonDown => Input.GetMouseButtonDown(0);

    public bool[] numberInputs = new bool[9];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        for (int i = 0; i < numberInputs.Length; i++)
        {
            numberInputs[i] = Input.GetKeyDown(KeyCode.Alpha1 + i);
        }
        
    }
}
