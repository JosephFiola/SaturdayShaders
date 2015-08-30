using UnityEngine;
using System.Collections;

public class MovePosition_Sine : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float zPos = Mathf.Sin(Time.time * speed);
		transform.position = new Vector3(0.0f, 0.0f, zPos-2.0f);

	
	}
}
