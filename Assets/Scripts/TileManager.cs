using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour 
{
    // SerializeFields - Assigned in-editor
    /// <summary>
    /// Tile dimensions.
    /// </summary>
    [Tooltip("Tile dimensions.")]
    [SerializeField]
    float tileDimensions = 5f;
    /// <summary>
    /// Reference to generic tile prefab.
    /// </summary>
    [Tooltip("Reference to generic tile prefab.")]
    [SerializeField]
    GameObject tilePrefab;
    /// <summary>
    /// Number of rows and columns of tiles to generate.
    /// Will generate value^2 tiles.
    /// </summary>
    [Tooltip("Number of rows and columns of tiles to generate.\nWill generate value^2 tiles.")]
    [SerializeField]
    int tileGridDimensions = 10;
    /// <summary>
    /// List of possible materials for instantiated tiles.
    /// </summary>
    [Tooltip("List of possible materials for instantiated tiles.")]
    [SerializeField]
    List<Material> tileMaterials;

    // Private fields
    /// <summary>
    /// X-position of tile.
    /// </summary>
    float xPosition;
    /// <summary>
    /// Y-position of tile.
    /// </summary>
    float yPosition;
    /// <summary>
    /// Z-position of tile.
    /// </summary>
    float zPosition;
    /// <summary>
    /// Spotlight attached to a tile.
    /// </summary>
    Light tileLight;
    /// <summary>
    /// List of instantiated tiles.
    /// </summary>
    List<GameObject> tiles = new List<GameObject>();
    /// <summary>
    /// List of tile material references.
    /// </summary>
    List<int> activeMaterials = new List<int>();

	// Use this for initialization
	void Start () 
	{
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        zPosition = transform.position.z;
        InstantiateTiles();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    /// <summary>
    /// Instantiates a grid of tiles with a random selection of materials.
    /// </summary>
    void InstantiateTiles()
    {
        int activeTile = 0;
        for(int z = 0; z < tileGridDimensions; z++)
        {
            for(int x = 0; x < tileGridDimensions; x++)
            {
                tiles.Add(Instantiate(tilePrefab));
                tiles[activeTile].transform.position = new Vector3(xPosition, yPosition, zPosition);
                activeMaterials.Add(Random.Range(0, tileMaterials.Count));
                tiles[activeTile].GetComponent<MeshRenderer>().material = tileMaterials[activeMaterials[activeTile]];
                tileLight = tiles[activeTile].GetComponentInChildren<Light>();
                if (tileLight != null)
                    tileLight.color = tileMaterials[activeMaterials[activeTile]].color;

                xPosition += tileDimensions;
                activeTile++;
            }
            xPosition = tileDimensions / 2;
            zPosition -= tileDimensions;
        }
    }
}
