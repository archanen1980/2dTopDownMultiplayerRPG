using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] private GameObject backgroundTilePrefab;
    [SerializeField] private int gridSize = 100;
    [SerializeField] private float tileSize = 1;

    void Start()
    {
        CreateBackgroundGrid();
    }

    void CreateBackgroundGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                GameObject tile = Instantiate(backgroundTilePrefab, position, Quaternion.identity);
                tile.transform.parent = transform;
            }
        }
    }
}
