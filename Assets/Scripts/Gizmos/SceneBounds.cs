using UnityEngine;

public class SceneBounds : MonoBehaviour
{
    public Transform bottomLeft;
    public Transform topRight;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 bottomRight = new Vector3(topRight.position.x, bottomLeft.position.y);
        Vector3 topLeft = new Vector3(bottomLeft.position.x, topRight.position.y);
        Gizmos.DrawLine(bottomLeft.position, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight.position);
        Gizmos.DrawLine(topRight.position, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft.position);
    }
}
