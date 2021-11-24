using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Spawn and Control Bricks
public class BricksController : NetworkBehaviour
{
    // Singleton Instance
    private static BricksController _instance;
    public static BricksController Instance { get { return _instance; } }

    // Brick Prefab and Associated Stuff
    [SerializeField]
    private GameObject brickPrefab;
    [SerializeField]
    private Material[] brickMaterials;

    // Spawning Variables
    [SerializeField] [Range(0f, 1f)]
    float brickGap = 0.5f; // Gap between bricks
    [SerializeField]
    Transform spawnTL; // Top-Left of Spawn Zone as set in Scene
    [SerializeField]
    Transform spawnBR; // Bottom-Right of Spawn Zone as set in Scene
    [SerializeField] [Range(1, 10)]
    private int numRows = 5;
    [SerializeField] [Range(1, 20)]
    private int numCols = 10;

    private List<Brick> brickPool = new List<Brick>(); // Store Bricks
    private int bricksActive = 0;
    public int BricksActive { get { return bricksActive; } }

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    // Spawn Bricks on Server
    [ClientRpc]
    public void RpcSpawnBricks()
    {
        // Store for readability
        float spawnZoneTop = spawnTL.position.y;
        float spawnZoneBottom = spawnBR.position.y;
        float spawnZoneLeft = spawnTL.position.x;
        float spawnZoneRight = spawnBR.position.x;
        
        // Dynamic brick size determined by spawn bounds, number of rows and columns, and gap size
        float brickHeight = (spawnZoneTop - spawnZoneBottom - brickGap * (numRows + 1)) / numRows; // (spawn zone height - total gap height) / rows
        float brickWidth = (spawnZoneRight - spawnZoneLeft - brickGap * (numCols + 1)) / numCols; // (spawn zone width - total gap width) / cols

        // Spawn each brick
        for (int row = 0; row < numRows; row++)
        {
            // top - half height - row offset - gap offset
            float yPos = spawnZoneTop - 0.5f * brickHeight - row * brickHeight - (row + 1) * brickGap;

            for (int col = 0; col < numCols; col++)
            {
                // left + half width + col offset + gap offset
                float xPos = spawnZoneLeft + 0.5f * brickWidth + col * brickWidth + (col + 1) * brickGap;

                // Spawn brick
                GameObject brick = Instantiate(brickPrefab, new Vector3(xPos, yPos), Quaternion.identity, transform);

                NetworkServer.Spawn(brick);

                brick.transform.localScale = new Vector3(brickWidth, brickHeight, 1f);
                // brick.GetComponent<Brick>().RpcSetMaterial(brickMaterials[row % brickMaterials.Length]); // Set Colour: mod to repeat colours for > 5 rows
                brick.GetComponent<MeshRenderer>().material = brickMaterials[row % brickMaterials.Length];
                brickPool.Add(brick.GetComponent<Brick>());

                
            }
        }

        bricksActive = brickPool.Count;
    }

    [Server]
    public void ReplaceBricks()
    {
        foreach (Brick brick in brickPool) brick.Enable();
        bricksActive = brickPool.Count;
    }

    [Server]
    public void DecrementBricksActive()
    {
        bricksActive--;

        if (bricksActive <= 0) GameController.Instance.NextRound();
    }
}
