using UnityEngine;
using System.Collections;

public class removeObject : MonoBehaviour {

	// we will need to access and make changes to our physical inventory
	GameObject physicalInventory;
	public float distance; // minimum distance to pickup object, limits distance of raycast
	void Start () {
		// phyiscalInventory is set to reference the game object named playerInventory, this object contains all the 
		// details regarding a user's current inventory
		physicalInventory = GameObject.Find ("PlayerInventory");
	}

	void Update () {
		// if the 'e' key is pressed, the user wants to pickup object
		if (Input.GetKeyDown (KeyCode.E)) {
			// the middle of the screen needs to be determined
			// this is what the user is pointing at
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			// a ray is shot from the center of the screen directly to the object the user
			// is looking at, this is to determine what they want to pickup
			Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit; // used to store information returned from raycast
			// the ray is shot, the script needs to remember the object hit by the array 
			if (Physics.Raycast(ray, out hit, distance)) {
				// a removeable object is created and set to reference the removeable
				// script of the object that the raycast hit, all removeable objects have this script
				removeable r = hit.collider.GetComponent<removeable>();
				// if the object hit contains a removeable script (r is not null) the object is removeable
				if(r != null) {
					// NOTE** objects connected together to be removed need to be tagged together
					// removeable will have a variable called tagID, the leave component will
					// have a tag which is that number allowing us to find the component and remove it
					// add object to inventory and change object in game world
					physicalInventory.GetComponent<inventory>().addItem(r.inventoryID);
					// destroy the object hit by the raycast after it has been added to the inventory
					Destroy(hit.transform.gameObject);
				}
			}
		}
	}
}
