using UnityEngine;
using System.Collections;
using System.Collections.Generic; // gives access to lists

public class itemDatabase : MonoBehaviour {
	// the item database will be a list containing all possible items the user can have
	// this will be a list of type items
	public List<item> items = new List<item>();
	// the size can be set through the inspector and then items can then be added manually

	void Start() {
		// items can be added to the database here using the item constructor
	}
}