using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawn and Control Bricks
public class BricksController : MonoBehaviour
{
    // Brick Prefab and Associated Stuff
    [SerializeField]
    private GameObject brickPrefab;
    [SerializeField]
    private Material[] brickMaterials;

    // Spawning Variables
    [SerializeField] [Range(0f, 1f)]
    float brickGap = 0.5f;
    [SerializeField]
    Transform spawnTL; // Top-Left of Spawn Zone as set in Scene
    [SerializeField]
    Transform spawnBR; // Bottom-Right of Spawn Zone as set in Scene
    [SerializeField] [Range(1, 10)]
    private int numRows = 5;
    [SerializeField] [Range(1, 20)]
    private int numCols = 10;
    
    private void Start()
    {
        // Store for readability
        float spawnZoneTop = spawnTL.position.y;
        float spawnZoneBottom = spawnBR.position.y;
        float spawnZoneLeft = spawnTL.position.x;
        float spawnZoneRight = spawnBR.position.x;
        
        float brickHeight = (spawnZoneTop - spawnZoneBottom - brickGap * (numRows + 1)) / numRows; // (spawn zone height - total gap height) / rows
        float brickWidth = (spawnZoneRight - spawnZoneLeft - brickGap * (numCols + 1)) / numCols; // (spawn zone width - total gap width) / cols

        for (int row = 0; row < numRows; row++)
        {
            float yPos = spawnZoneTop - 0.5f * brickHeight - row * brickHeight - (row + 1) * brickGap;

            for (int col = 0; col < numCols; col++)
            {
                float xPos = spawnZoneLeft + 0.5f * brickWidth + col * brickWidth + (col + 1) * brickGap;

                GameObject brick = Instantiate(brickPrefab, new Vector3(xPos, yPos), Quaternion.identity, transform);
                brick.transform.localScale = new Vector3(brickWidth, brickHeight, 1f);
                brick.GetComponent<Brick>().SetMaterial(brickMaterials[row % brickMaterials.Length]); // modulus to repeat colours for > 5 rows
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
