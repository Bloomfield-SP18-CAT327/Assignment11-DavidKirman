using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SyncVar]
	public Color color;

	public GameObject bulletPrefab;

	[SerializeField]
	private float moveSpeed = 2f;

	public override void OnStartClient ()
	{
		GetComponent<Renderer> ().material.color = color;
	}

	private void Update()
	{
		if (isLocalPlayer && hasAuthority) {
			GetInput ();
		}
	}

	void GetInput()
	{
		float x = Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime;
		float y = Input.GetAxisRaw ("Vertical") * moveSpeed * Time.deltaTime;

		if (Input.GetButtonUp ("Fire1")) {
			CmdFire ();
		}
		if (isServer) {
			RpcMoveIt (x, y);
		}
		else {
			CmdMoveIt (x, y);
		}
	}

	//If the game is on the actual server
	[ClientRpc]
	void RpcMoveIt(float x, float y)
	{
		transform.Translate (x, y, 0);
	}


	//Command Attribute
	// This is going to go through the server, tells the server to run these methods
	// Tells server that all players should do this method in that world
	[Command]
	void CmdMoveIt(float x, float y)
	{
		RpcMoveIt(x, y);
	}

	[Command]
	public void CmdFire()
	{
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, this.transform.position + this.transform.right, Quaternion.identity);
		bullet.GetComponent<Rigidbody> ().velocity = Vector3.forward * 17.5f;
		bullet.GetComponent<Bullet> ().color = color;
		Destroy (bullet, 1f);
		NetworkServer.Spawn (bullet);
	}
}
