using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour
{
    public GameObject emptyFloorTilePrefab;
    public GameObject[] floorTilePrefab;

    //FLOOR HEIGHT AND WIDTH
    float floorMinX = -8.9f;
    float floorMaxX =  8.9f;
    float floorY =  -3.0f;
    // Start is called before the first frame update
    void Start()
    {
        //GETS HEIGHT AND WIDTH OF TILES IN GAME COORDINATES
        Renderer r = emptyFloorTilePrefab.GetComponent<Renderer>();
        float tileWidth = r.bounds.size.x;
        float tileHeight = r.bounds.size.y;

        float newTileX = floorMinX + tileWidth / 2.0f;

        // while our tile X position is still within our boundaries
        while (newTileX < floorMaxX)
        {
         // calculate starting Y position of the bottom-most tile 
         // in this column
         float newTileY = floorY + tileHeight / 2.0f;
         
         GameObject prefab = emptyFloorTilePrefab;
         int index = Random.Range(0, floorTilePrefab.length);
         prefab = floorTilePrefab[index];

         Instantiate(prefab, new Vector3(newTileX, newTileY, 0), Quaternion.identity);

         newTileX += tileWidth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
