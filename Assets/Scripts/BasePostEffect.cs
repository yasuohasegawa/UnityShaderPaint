using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public abstract class BasePostEffect : MonoBehaviour
{
	private Material material;

	public abstract string ShaderName { get; }

	protected Material Material { get { return material; } }

	protected virtual void Awake() {
		Shader shader = Shader.Find( ShaderName );
		material = new Material( shader );
	}

	protected virtual void Start() {
		
	}

	protected virtual void OnRenderImage( RenderTexture src, RenderTexture dest ) {
		Graphics.Blit( src, dest, material );
	}
}
