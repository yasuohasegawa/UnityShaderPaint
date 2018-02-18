using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum eBlushType {
	Type1,
	Type2,
	Type3
}
	
public enum eInteraction {
	None,
	ParamUpdated
}

public class DrawingEffect : BasePostEffect {
	private Vector3 position;
	private Vector3 screenToWorldPointPosition;
	private Vector4 mousepos = Vector4.zero;
	private Vector4 currentColor = new Vector4 (1.0f, 1.0f, 1.0f, 1.0f);
	private eBlushType currentBlushType = eBlushType.Type1;
	private eInteraction interactionState = eInteraction.None;

	private int sMouseID;
	private int sBrushSizeID;
	private int sDrawFlgID;
	private int sBlushTypeID;
	private int sColorID;
	private int sBlurID;
	private int sBlurValueID;

	[SerializeField]
	private List<Toggle> colors = new List<Toggle> ();

	[SerializeField]
	private List<Toggle> brushes = new List<Toggle> ();

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private Slider blurSlider;

	[SerializeField]
	private Toggle blurToggle;

	public override string ShaderName
	{
		get { return "Custom/Drawing"; }
	}

	protected override void Start() {
		sMouseID = Shader.PropertyToID("_mouse");
		sDrawFlgID = Shader.PropertyToID("_DrawFlg");
		sBrushSizeID = Shader.PropertyToID("_BrushSize");
		sBlushTypeID = Shader.PropertyToID("_blushType");
		sDrawFlgID = Shader.PropertyToID("_DrawFlg");
		sColorID = Shader.PropertyToID("_selectedColor");
		sBlurID = Shader.PropertyToID("_Blur");
		sBlurValueID = Shader.PropertyToID("_BlurValue");

		Material.SetInt (sDrawFlgID, 0);
		Material.SetVector (sColorID, currentColor);
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
				return;
			
			position = Input.mousePosition;
			position.z = 10.0f;
			screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);
			Vector3 screenPos = Camera.main.WorldToViewportPoint (screenToWorldPointPosition);
			mousepos.x = screenPos.x;
			mousepos.y = screenPos.y;

			if (interactionState == eInteraction.None) {
				Material.SetFloat (sBrushSizeID, slider.value);
				Material.SetVector (sColorID, currentColor);
				Material.SetInt (sDrawFlgID, 1);
				Material.SetInt (sBlushTypeID, (int)currentBlushType);
				Material.SetFloat (sBlurValueID, blurSlider.value);
				Material.SetInt (sBlurID, blurToggle.isOn ? 1 : 0);
				interactionState = eInteraction.ParamUpdated;
			}

			Material.SetVector (sMouseID, mousepos);
		} else {
			interactionState = eInteraction.None;
		}

		if (brushes [0].isOn) {
			currentBlushType = eBlushType.Type1;
		} else if (brushes [1].isOn) {
			currentBlushType = eBlushType.Type2;
		} else if (brushes [2].isOn) {
			currentBlushType = eBlushType.Type3;
		}
	}

	public void UpdateCurrentColor(float r, float g, float b) {
		currentColor.x = r;
		currentColor.y = g;
		currentColor.z = b;
	}

	/* UI things */

	public void OnClear() {
		Material.SetInt (sDrawFlgID, 0);
	}

	public void OnToggleColor() {
		if (colors [0].isOn) {
			currentColor.x = 1.0f;
			currentColor.y = 1.0f;
			currentColor.z = 1.0f;
		} else if (colors [1].isOn) {
			currentColor.x = 0.0f;
			currentColor.y = 1.0f;
			currentColor.z = 1.0f;
		} else if (colors [2].isOn) {
			currentColor.x = 1.0f;
			currentColor.y = 1.0f;
			currentColor.z = 0.0f;
		} else if (colors [3].isOn) {
			currentColor.x = 1.0f;
			currentColor.y = 0.0f;
			currentColor.z = 1.0f;
		} else if (colors [4].isOn) {
			currentColor.x = 0.0f;
			currentColor.y = 0.0f;
			currentColor.z = 1.0f;
		}
	}
}
