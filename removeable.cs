using UnityEngine;
using System.Collections;

public class removeable : MonoBehaviour {
	// inventoryID will be the ID of the item that nee	ds to be added to the inventory once
	// the gameobject has been removed or destroyed
	public int inventoryID;
	// tagID will be the ID that links items such as the trees and the leaves
	// this will allow us to remove the trunk, search for the connected leaves and then remove them
	public string tagID;
}