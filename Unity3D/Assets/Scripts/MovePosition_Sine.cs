using UnityEngine;
using System.Collections;

public class MovePosition_Sine : MonoBehaviour {

	public float xSpeed;
	public float ySpeed;
	public float zSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		float xPos = Mathf.Sin (Time.time * xSpeed);
		float yPos = Mathf.Cos (Time.time * ySpeed);
		float zPos = Mathf.Sin (Time.time * zSpeed);
		transform.position = new Vector3(2.0f * xPos, yPos, zPos-2.0f);	
	}
}
