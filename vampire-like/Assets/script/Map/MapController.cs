using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour

{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
    }

    void ChunkChecker()
    {
        if(pm.moveDir.x >0 && pm.moveDir.y==0) //Right
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(40,0,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(40,0,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x <0 && pm.moveDir.y==0) //left
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(-40,0,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(-40,0,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x ==0 && pm.moveDir.y>0) //up
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(0,38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(0,38,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x ==0 && pm.moveDir.y<0) //down
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(0,-38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(0,-38,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x >0 && pm.moveDir.y>0) //Right up
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(40,38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(40,38,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x >0 && pm.moveDir.y<0) //Right down
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(40,-38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(40,-38,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x <0 && pm.moveDir.y>0) //left up
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(-40,38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(-40,38,0);
                SpawnChunk();
            }
        else if(pm.moveDir.x <0 && pm.moveDir.y<0) //Left Down
            if (!Physics2D.OverlapCircle(player.transform.position + new Vector3(-40,-38,0), checkerRadius, terrainMask))
            {
                noTerrainPosition= player.transform.position + new Vector3(-40,-38,0);
                SpawnChunk();
            }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
    }
}
