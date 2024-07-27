using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    /* public GameObject[] playerPrefabs;
     public Transform[] spawnPoints;

     private void Start()
     {
         int randomNumber = Random.Range(0, spawnPoints.Length);
         Transform spawnPoint = spawnPoints[randomNumber];
         GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
         PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

     }*/
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private static List<int> usedSpawnPoints = new List<int>();

    private void Start()
    {
        int spawnPointIndex = GetAvailableSpawnPoint();
        if (spawnPointIndex == -1)
        {
            Debug.LogError("No available spawn points!");
            return;
        }

        Transform spawnPoint = spawnPoints[spawnPointIndex];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, spawnPoint.rotation);
        
            RenderTexture rtx = Resources.Load<RenderTexture>("Minimap_rendertexture");

            transform.Find("Minimap_camera").GetComponent<Camera>().targetTexture = rtx;

            GameObject.Find("Minimap").GetComponent<RawImage>().texture = rtx;
            Debug.Log("player spawned successfully");
        
    }


    private int GetAvailableSpawnPoint()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!usedSpawnPoints.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count == 0)
        {
            return -1; // No available spawn points
        }

        int randomIndex = Random.Range(0, availableIndices.Count);
        int selectedSpawnPointIndex = availableIndices[randomIndex];
        usedSpawnPoints.Add(selectedSpawnPointIndex);

        return selectedSpawnPointIndex;
    }
}