using System.Drawing;
using UnityEngine;

public class Board : MonoBehaviour
{
    public WebcamController webcamController;
    public Vector2 boardSize = new Vector2(10, 10);

    public Vector3 bottomLeft;

    private void Awake()
    {
        bottomLeft = transform.position - new Vector3(boardSize.x, 0, boardSize.y) / 2f;
    }

    public Vector3 RandomPosition()
    {
        float x = Random.Range(bottomLeft.x, bottomLeft.x + boardSize.x);
        float z = Random.Range(bottomLeft.z, bottomLeft.z + boardSize.y);
        return new Vector3(x, transform.position.y, z);
    }

    public Vector3[] RandomBorderPositions()
    {
        Vector3[] borderPositions = new Vector3[2];
        int[] borderIds = new int[2];
        borderIds[0] = Random.Range(0, 4);
        borderIds[1] = (borderIds[0] + Random.Range(1, 4)) % 4;
        for (int i = 0; i < 2; i++)
        {
            float scale = Random.Range(0f, 1f);
            switch (borderIds[i])
            {
                case 0:
                    borderPositions[i] = new Vector3(
                        bottomLeft.x + boardSize.x * scale,
                        transform.position.y,
                        bottomLeft.z
                    );
                    break;
                case 1:
                    borderPositions[i] = new Vector3(
                        bottomLeft.x + boardSize.x,
                        transform.position.y,
                        bottomLeft.z + boardSize.y * scale
                    );
                    break;
                case 2:
                    borderPositions[i] = new Vector3(
                        bottomLeft.x + boardSize.x * scale,
                        transform.position.y,
                        bottomLeft.z + boardSize.y
                    );
                    break;
                case 3:
                     borderPositions[i] = new Vector3(
                        bottomLeft.x,
                        transform.position.y,
                        bottomLeft.z + boardSize.y * scale
                    );
                    break;
            }
        }
        return borderPositions;
    }

    public Vector3 WebcamToWorld(PointF point)
    {
        float x = bottomLeft.x + (point.X / 512.0f) * boardSize.x;
        float z = bottomLeft.z + ((512.0f - point.Y) / 512.0f) * boardSize.y;
        return new Vector3(x, transform.position.y, z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new UnityEngine.Color(0.2f, 0.3f, 1f, 0.5f);
        Gizmos.DrawCube(transform.position + Vector3.up * 0.3f, new Vector3(boardSize.x, 0.6f, boardSize.y));
    }

}
