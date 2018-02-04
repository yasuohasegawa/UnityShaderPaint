using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour, IPointerClickHandler {
	private GameObject canvas;
	private GameObject picker;
	private Texture2D tex;
	private Vector2 uiPos = new Vector2(-360,15);
	private Vector2 uiSize = new Vector2(256,256);

	private DrawingEffect drawing;

	// Use this for initialization
	void Start () {
		drawing = GameObject.Find("Main Camera").GetComponent<DrawingEffect> ();
		canvas = GameObject.Find("UI");
		picker = new GameObject("Picker");

		int w = 256, h = 256;
		tex = new Texture2D(w,h);
		for (int j = 0; j < h; j++) {
			for (int i = 0; i < w; i++) {
				tex.SetPixel(j, i, Color.HSVToRGB((float)j / 255f, 1f, 1f));
			}
		}
		tex.Apply();

		// ピッカーの設定
		picker.transform.parent = canvas.transform;
		Image img = picker.AddComponent<Image>();
		picker.GetComponent<RectTransform>().anchoredPosition = uiPos;
		picker.GetComponent<RectTransform>().sizeDelta = uiSize;
		img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
	}

	public void OnPointerClick(PointerEventData pointerEventData){
		Vector2 localPosition = GetLocalPosition(pointerEventData.position);
		int xx = (int)localPosition.x-(int)uiPos.x+(int)(uiSize.x/2);
		int yy = (int)(uiSize.y)-((int)localPosition.y-(int)uiPos.y+(int)(uiSize.y/2));
		Color col = tex.GetPixel (xx, yy);
		drawing.UpdateCurrentColor (1.0f-col.r, 1.0f-col.g, 1.0f-col.b);
	}

	private Vector2 GetLocalPosition(Vector2 screenPosition) {
		return transform.InverseTransformPoint(screenPosition);
	}

	// Update is called once per frame
	void Update () {

	}
}
