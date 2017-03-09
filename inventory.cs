using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class inventory : MonoBehaviour {
	// the inventory is a list of items
	public List<item> inventoryList = new List<item>();
	// the inventory will need to use the item database
	private itemDatabase database;
	public int maxInventory = 8;
	private bool showInventory;

	// we will need a set amount of slots for the inventory
	public int slotsX = 8;
	public int slotsY = 1;
	public List<item> slots = new List<item>();

	private int slotPosX = Screen.width/2 - 230;
	private int slotPosY = Screen.height - 100;

	// used to define skin for gui elements
	public GUISkin inventorySkin;


	void Start() {
		for (int i = 0; i < (slotsX * slotsY); i++) {
			// used to populate the slots list and inventory list
			slots.Add(new item());
			inventoryList.Add(new item());
		}
		// the item database object will contain a tag called item database
		// we need the item database component on the game object which contains the list of 
		// all game objects - this allows us to reference all needed items
		database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<itemDatabase>();
	}

	// OnGUI is a unity method for drawing gui elements on screen
	void OnGUI() {
		GUI.skin = inventorySkin;
		if (showInventory) {
			// inventory is drawn if the inventory should be shown
			DrawInventory();
		}
	}

	void DrawInventory () {
		int i = 0;
		for (int y = 0; y < slotsY; y++) {
			for (int x = 0; x < slotsX; x++) {
				Rect slotRect = new Rect(x * 60 + slotPosX, y * 60 + slotPosY, 50, 50);
				// need to create a GUI skin material and set up sprite to use it
				GUI.Box(slotRect, "", inventorySkin.GetStyle("Slot"));
				slots[i] = inventoryList[i];
				if (slots[i].name != null) {
					GUI.DrawTexture(slotRect, slots[i].icon);
				}
				i++;
			}
		}
	}

	public void addItem(int ID) {
		// loop through all elements in inventory list
		for (int i = 0; i < inventoryList.Count; i++) {
			// empty inventory slot is found
			print (i);
			if (inventoryList[i].name == null) {
				// need to find the item from database to add to inventory
				for (int a = 0; a < database.items.Count; a++) {
					// when item with the correct ID is found, item in inventory is set to reference
					// a database item
					if (database.items[a].ID == ID) {
						inventoryList[i] = database.items[a];
						print (i);
						print (database.items [a].name);
						print (inventoryList [i].name);
					}
				}
				break;
			}
		}
		print (inventoryList [0].name);
	}

	void Update() {
		// if input button is pressed
		if (Input.GetKey("i")) {
			// inventory shown will be opposite of current state
			// if it is not shown, it will now be shown and vice versa
			showInventory = !showInventory;
		}
	}

}