using UnityEngine;

public class FakeCursor : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 xyPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = xyPos;
    }
}
