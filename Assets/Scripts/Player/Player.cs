using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string Id;
	public string Name;
	public TMPro.TextMeshPro mPro;
	public SpriteRenderer renderer;
	private PlayerMovement playerMovement;
	private void OnEnable()
	{
		renderer = GetComponent<SpriteRenderer>();
		playerMovement = GetComponent<PlayerMovement>();
	}
	public void SetRender(Sprite sprite)
	{
		renderer.sprite = sprite;
	}
	public void Init(string id, string name)
	{
		this.Id = id;
		this.Name = name;
		mPro.text = name;
		playerMovement.id = id;
	}
}
