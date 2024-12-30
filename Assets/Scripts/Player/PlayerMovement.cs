using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
	private Rigidbody2D rb;
	private ClientManager clientManager;
	public string id;
	public float _interval=0.05f;
	public float lastTime;
	public Vector3 targetPos;
	ServerService service;
	private void Start()
	{
		service=ServerService.Instance;
		moveSpeed = 7f;
		clientManager = ClientManager.instance;
		rb = GetComponent<Rigidbody2D>();
		service.SubscribeOperationHandler<PlayerInput>(ServerToClientOperationCode.UpdatePlayerPos, OnSyncPlayerPos);
	}

	private void OnSyncPlayerPos(PlayerInput data)
	{
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			Debug.Log($"Get player input ID {data.PlayerID} Pos {data.Direction} currentId {id}");
			if (data.PlayerID.Equals(id))
			{
				rb.velocity = targetPos;
			}
		});
	
	}

	// Update is called once per frame
	void Update()
    {
		if(clientManager.IsClientConnect&&clientManager.tCPClientChat.clientID.Equals(id))
		{
			float moveX = Input.GetAxis("Horizontal");
			float moveZ = Input.GetAxis("Vertical");
			if (Time.time > _interval + lastTime)
			{
				lastTime += _interval;
				Debug.Log($"Move {moveX}");
				if(moveX==0)return;
				//rb.velocity += new Vector2(moveX, moveZ) * moveSpeed * Time.deltaTime;
				targetPos+= new Vector3(moveX, moveZ,0) * moveSpeed * Time.deltaTime;
				PlayerInput playerInput = new PlayerInput() { 
					PlayerID = id,
					Direction=targetPos
				};
				service.SendUpdatePlayerPos(playerInput, ClientToServerOperationCode.UpdatePlayerPos);
			}
		}
		
	}
}
