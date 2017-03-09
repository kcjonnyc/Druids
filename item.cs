using UnityEngine;
using System.Collections;

// allows for the properties of item to be displayed individually in the unity inspector
[System.Serializable]

public class item {
	public string name;
	public int ID;
	public string description;
	public Texture2D icon;

	// item constructor for use outside of inspector
	public item(string itemName, int itemID, string desc) {
		name = itemName;
		ID = itemID;
		description = desc;
		// load icon with the same name from resources folder
		icon = Resources.Load<Texture2D>("Item Icons/" + itemName);
	}

	// item constructor to create an empty/default item
	public item () {

	}
}