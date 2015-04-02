using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {
	public string suit;
	public int rank;
	public Color color = Color.black;
	public string colS = "Black";

	public List<GameObject> decoGOs = new List<GameObject>();
	public List<GameObject> pipGOs = new List<GameObject> ();

	public GameObject back;

	public CardDefinition def;

	public SpriteRenderer[] spriteRenderers;

	// Use this for initialization
	void Start () {
		SetSortOrder (1);
	}
	virtual public void OnMouseUpAsButton(){
				print (name);
		}

	public bool faceUp {
				get {
						return(!back.activeSelf);
				}
				set {
						back.SetActive (!value);
				}
		}
	// If spriteRenderers is not yet defined, this function defines it
		public void PopulateSpriteRenderers() {
			// If spriteRenderers is null or empty
			if (spriteRenderers == null || spriteRenderers.Length == 0) {
				// Get SpriteRenderer Components of this GameObject and its children
				spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
			}
		}

	// Sets the sortingLayerName on all SpriteRenderer Components
	public void SetSortingLayerName(string tSLN) {
		PopulateSpriteRenderers();
		foreach (SpriteRenderer tSR in spriteRenderers) {
			tSR.sortingLayerName = tSLN;
		}
	}

	// Sets the sortingOrder of all SpriteRenderer Components
	public void SetSortOrder(int sOrd) {
		PopulateSpriteRenderers();
		// The white background of the card is on bottom (sOrd)
		// On top of that are all the pips, decorators, face, etc. (sOrd+1)
		// The back is on top so that when visisble, it covers the rest (sOrd+2)
		// Iterate through all the spriteRenderers as tSR
		foreach (SpriteRenderer tSR in spriteRenderers) {
			if (tSR.gameObject == this.gameObject) {
				// If the gameObject is this.gameObject, it's the background
				tSR.sortingOrder = sOrd; // Set its order to sOrd
				continue; // And continue to the next iteration of the loop
			}// Each of the children of this GameObject are named
					// switch based on the names
				switch (tSR.gameObject.name) {
					case "back": // if the name is "back"
					tSR.sortingOrder = sOrd+2;
					// ^ Set it to the highest layer to cover everything else
					break;
					case "face": // if the name is "face"
					default: // or if it's anything else
					tSR.sortingOrder = sOrd+1;
					// ^ Set it to the middle layer to be above the background
					break;
				}
		}
	}



	

}
[System.Serializable]
public class Decorator{
	public string type;
	public Vector3 loc;
	public bool flip = false;
	public float scale = 1f;
}
[System.Serializable]
public class CardDefinition{
	public string face;
	public int rank;
	public string suit;
	public List<Decorator> pips = new List<Decorator>();
}


