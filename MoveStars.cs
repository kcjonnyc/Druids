using UnityEngine;
using System.Collections;

public class MoveStars : MonoBehaviour {

	// Takes the positiom, rotation and scale of the stars
	public Transform stars;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// the rotation of the stars object is set to the
		// rotation of this object (the sun)
		stars.transform.rotation = transform.rotation;
	}
}
