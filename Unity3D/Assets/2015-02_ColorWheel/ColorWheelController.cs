using UnityEngine;
using System.Collections;





public class ColorWheelController : MonoBehaviour {


	public float rotSpeed = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


		float rotValue = Mathf.Sin(Time.time * rotSpeed);


		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;

		mat.SetFloat("_Rotate", rotValue);

	}
}
