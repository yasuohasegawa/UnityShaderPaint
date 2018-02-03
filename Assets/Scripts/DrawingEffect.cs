using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum eColorType {
	Black = 0,
	Red = 1,
	Blue = 2,
	Green = 3,
	Yellow = 4
}

public enum eBlushType {
	Type1 = 0,
	Type2 = 1,
}

public class DrawingEffect : BasePostEffect {
	private Vector3 position;
	private Vector3 screenToWorldPointPosition;
	private Vector4 mousepos = Vector4.zero;
	private eColorType currentColorType = eColorType.Black;
	private eBlushType currentBlushType = eBlushType.Type1;

	[SerializeField]
	private List<Toggle> colors = new List<Toggle> ();

	[SerializeField]
	private List<Toggle> brushes = new List<Toggle> ();

	[SerializeField]
	private Slider slider;

	public override string ShaderName
	{
		get { return "Custom/Drawing"; }
	}

	protected override void Start() {
		Material.SetInt ("_DrawFlg", 0);
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
			position = Input.mousePosition;
			position.z = 10.0f;
			screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);
			Vector3 screenPos = Camera.main.WorldToViewportPoint (screenToWorldPointPosition);
			mousepos.x = screenPos.x;
			mousepos.y = screenPos.y;

			Material.SetFloat ("_BrushSize", slider.value);
			Material.SetInt ("_colorType", (int)currentColorType);
			Material.SetVector ("_mouse", mousepos);
			Material.SetInt ("_DrawFlg", 1);
			Material.SetInt ("_blushType", (int)currentBlushType);
		}

		if (colors [0].isOn) {
			currentColorType = eColorType.Black;
		} else if (colors [1].isOn) {
			currentColorType = eColorType.Red;
		} else if (colors [2].isOn) {
			currentColorType = eColorType.Blue;
		} else if (colors [3].isOn) {
			currentColorType = eColorType.Green;
		} else if (colors [4].isOn) {
			currentColorType = eColorType.Yellow;
		}

		if(brushes[0].isOn) {
			currentBlushType = eBlushType.Type1;
		} else if(brushes[1].isOn) {
			currentBlushType = eBlushType.Type2;
		}
	}

	/* UI things */

	public void OnClear() {
		Material.SetInt ("_DrawFlg", 0);
	}
}
