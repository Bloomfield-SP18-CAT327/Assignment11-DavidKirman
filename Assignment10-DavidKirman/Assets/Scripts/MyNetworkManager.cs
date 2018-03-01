using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		GameObject playerToSpawn = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		playerToSpawn.GetComponent<Player> ().color = new Color (Random.Range (0, 1), Random.Range (0, 1), Random.Range (0, 1));
		NetworkServer.AddPlayerForConnection (conn, playerToSpawn, playerControllerId);
	}
}
