using UnityEngine;
using System.Collections;


public class _03_ShaderController : MonoBehaviour {



	public float leftAmp	= 0.1f;
	public float rightAmp	= 0.9f;
	public float topAmp		= 0.1f;
	public float bottomAmp	= 0.9f;

	public float leftSpeed		= 0.0f;
	public float rightSpeed		= 0.0f;
	public float topSpeed		= 0.0f;
	public float bottomSpeed	= 0.0f;

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 20), "Left Edge Amplitude");
		leftAmp = GUI.HorizontalSlider(new Rect(10, 30, 100, 20), leftAmp, 0.0F, 1.0F);
		GUI.Label(new Rect(10, 50, 200, 20), "Right Edge Amplitude");
		rightAmp = GUI.HorizontalSlider(new Rect(10, 70, 100, 20), rightAmp, 0.0F, 1.0F);
		GUI.Label(new Rect(10, 90, 200, 20), "Top Edge Amplitude");
		topAmp = GUI.HorizontalSlider(new Rect(10, 110, 100, 20), topAmp, 0.0F, 1.0F);
		GUI.Label(new Rect(10, 130, 200, 20), "Bottom Edge Amplitude");
		bottomAmp = GUI.HorizontalSlider(new Rect(10, 150, 100, 20), bottomAmp, 0.0F, 1.0F);


		GUI.Label(new Rect(10, 170, 100, 20), "LeftSpeed");
		leftSpeed = GUI.HorizontalSlider(new Rect(10, 190, 100, 20), leftSpeed, 0.0F, 20.0F);
		GUI.Label(new Rect(10, 210, 100, 20), "RightSpeed");
		rightSpeed = GUI.HorizontalSlider(new Rect(10, 230, 100, 20), rightSpeed, 0.0F, 20.0F);
		GUI.Label(new Rect(10, 250, 100, 20), "TopSpeed");
		topSpeed = GUI.HorizontalSlider(new Rect(10, 270, 100, 20), topSpeed, 0.0F, 20.0F);
		GUI.Label(new Rect(10, 290, 100, 20), "BottomSpeed");
		bottomSpeed = GUI.HorizontalSlider(new Rect(10, 310, 100, 20), bottomSpeed, 0.0F, 20.0F);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float leftValue = leftAmp	*	Mathf.Abs(Mathf.Sin(Time.time * leftSpeed));
		float rightValue =	rightAmp *	Mathf.Abs(Mathf.Cos(Time.time * rightSpeed));
		float topValue =	topAmp	*	Mathf.Abs(Mathf.Sin(Time.time * topSpeed));
		float bottomValue =	bottomAmp *	Mathf.Abs(Mathf.Cos(Time.time * bottomSpeed));

		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;

		mat.SetFloat("_Left", leftValue);
		mat.SetFloat("_Right", rightValue);
		mat.SetFloat("_Top", topValue);
		mat.SetFloat("_Bottom", bottomValue);

	}
}
