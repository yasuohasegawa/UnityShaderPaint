using UnityEngine;
using System.Collections;

public class DrawingEffect : BasePostEffect {
	private Vector3 position;
	private Vector3 screenToWorldPointPosition;
	private Vector4 mousepos = Vector4.zero;

	public override string ShaderName
	{
		get { return "Custom/Drawing"; }
	}

	void Start() {
		
	}

	void Update() {
		position = Input.mousePosition;
		position.z = 10.0f;
		screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
		Vector3 screenPos = Camera.main.WorldToViewportPoint (screenToWorldPointPosition);
		mousepos.x = screenPos.x;
		mousepos.y = screenPos.y;

		Material.SetVector ("_mouse", mousepos);
	}

}
