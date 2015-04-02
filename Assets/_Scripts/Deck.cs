﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
	//Suits
	public Sprite suitClub;
	public Sprite suitDiamond;
	public Sprite suitHeart;
	public Sprite suitSpade;

	public Sprite[] faceSprites;
	public Sprite[] rankSprites;

	public Sprite cardBack;
	public Sprite cardBackGold;
	public Sprite cardFront;
	public Sprite cardFrontGold;

	//Prefabs
	public GameObject prefabSprite;
	public GameObject prefabCard;


	public bool _________________;

	public PT_XMLReader xmlr;
	public List<string> cardNames;
	public List<Card> cards;
	public List<Decorator> decorators;
	public List<CardDefinition> cardDefs;
	public Transform deckAnchor;
	public Dictionary<string,Sprite> dictSuits;


	public void InitDeck(string deckXMLText){
		if(GameObject.Find ("_Deck") == null){
			GameObject anchorGO = new GameObject("_Deck");
			deckAnchor = anchorGO.transform;
			}

			dictSuits = new Dictionary<string, Sprite>(){
			{"C", suitClub},
			{"D", suitDiamond},
			{"H", suitHeart},
			{"S", suitSpade}
		};
				ReadDeck (deckXMLText);
		MakeCards();
		}

	public void ReadDeck(string deckXMLText){
		xmlr = new PT_XMLReader ();
		xmlr.Parse (deckXMLText);

		string s = "xml[0] decorator[0]";
		s += "type = " + xmlr.xml["xml"][0]["decorator"][0].att("type");
		s += "x = " +xmlr.xml["xml"][0]["decorator"][0].att ("x");
		s += "y = " + xmlr.xml["xml"][0]["decorator"][0].att ("y");
	    s += "scale = " + xmlr.xml["xml"][0]["decorator"][0].att("scale");
		                          //print (s);
		decorators = new List<Decorator> ();
		PT_XMLHashList xDecos = xmlr.xml ["xml"] [0] ["decorator"];
		Decorator deco;
		for (int i = 0; i < xDecos.Count; i++) {
						deco = new Decorator ();
						deco.type = xDecos [i].att ("type");
						deco.flip = (xDecos [i].att ("flip") == "1");
						deco.scale = float.Parse (xDecos [i].att ("scale"));
						deco.loc.x = float.Parse (xDecos [i].att ("x"));
						deco.loc.y = float.Parse (xDecos [i].att ("y"));
						deco.loc.z = float.Parse (xDecos [i].att ("z"));
						decorators.Add (deco);
				}

		cardDefs = new List<CardDefinition> ();
		PT_XMLHashList xCardDefs = xmlr.xml ["xml"] [0] ["card"];
		for (int i = 0; i < xCardDefs.Count; i++) {
						CardDefinition cDef = new CardDefinition ();
						cDef.rank = int.Parse (xCardDefs [i].att ("rank"));
						PT_XMLHashList xPips = xCardDefs [i] ["pip"];
						if (xPips != null) {
								for (int j = 0; j < xPips.Count; j++) {
										deco = new Decorator ();
										deco.type = "pip";
										deco.flip = (xPips [j].att ("flip") == "1");
										deco.loc.x = float.Parse (xPips [j].att ("x"));
										deco.loc.y = float.Parse (xPips [j].att ("y"));
										deco.loc.z = float.Parse (xPips [j].att ("z"));
										if (xPips [j].HasAtt ("scale")) {
												deco.scale = float.Parse (xPips [j].att ("scale"));
										}
										cDef.pips.Add (deco);
								}
						}
						if (xCardDefs [i].HasAtt ("face")) {
								cDef.face = xCardDefs [i].att ("face");
						}
						cardDefs.Add (cDef);
				}
			              
		                          }
		public CardDefinition GetCardDefinitionByRank(int rnk){
			foreach(CardDefinition cd in cardDefs){
				if(cd.rank == rnk){
					return(cd);
				}
			}
			return(null);
		}


		// Make the Card GameObjects
	public void MakeCards() {
		// cardNames will be the names of cards to build
		// Each suit goes from 1 to 13 (e.g., C1 to C13 for Clubs)
		cardNames = new List<string>();
		string[] letters = new string[] {"C","D","H","S"};
		foreach (string s in letters) {
			for (int i=0; i<13; i++) {
				cardNames.Add(s+(i+1));
			}
		}
			// Make a List to hold all the cards
				cards = new List<Card>();
		// Several variables that will be reused several times
		Sprite tS = null;
		GameObject tGO = null;
		SpriteRenderer tSR = null;
		// Iterate through all of the card names that were just made
		for (int i=0; i<cardNames.Count; i++) {
			// Create a new Card GameObject
			GameObject cgo = Instantiate(prefabCard) as GameObject;
			// Set the transform.parent of the new card to the anchor.
			cgo.transform.parent = deckAnchor;
			Card card = cgo.GetComponent<Card>(); // Get the Card Component
			// This just stacks the cards so that they're all in nice rows
			cgo.transform.localPosition = new Vector3( (i%13)*3, i/13*4, 0 );
			// Assign basic values to the Card
			card.name = cardNames[i];
			card.suit = card.name[0].ToString();
			card.rank = int.Parse( card.name.Substring(1) );
			if (card.suit == "D" || card.suit == "H") {
				card.colS = "Red";
				card.color = Color.red;
			}

						card.def = GetCardDefinitionByRank (card.rank);

						foreach (Decorator deco in decorators) {
								if (deco.type == "suit") {
												tGO = Instantiate (prefabSprite) as GameObject;
												tSR = tGO.GetComponent<SpriteRenderer> ();
												tSR.sprite = dictSuits [card.suit];
										} else {
												tGO = Instantiate (prefabSprite) as GameObject;
												tSR = tGO.GetComponent<SpriteRenderer> ();
												tS = rankSprites [card.rank];
												tSR.sprite = tS;
												tSR.color = card.color;
										}
										tSR.sortingOrder = 1;
										tGO.transform.parent = cgo.transform;
										tGO.transform.localPosition = deco.loc;
										if (deco.flip) {
												tGO.transform.rotation = Quaternion.Euler (0, 0, 180);
										}
										if (deco.scale != 1) {
												tGO.transform.localScale = Vector3.one * deco.scale;
										}
										tGO.name = deco.type;
										card.decoGOs.Add (tGO);
								}

				foreach(Decorator pip in card.def.pips){
						tGO = Instantiate(prefabSprite) as GameObject;
						tGO.transform.parent = cgo.transform;
						tGO.transform.localPosition = pip.loc;
				if(pip.flip){
					tGO.transform.rotation = Quaternion.Euler(0,0,180);
				}

				if(pip.scale != 1){
					tGO.transform.localScale = Vector3.one * pip.scale;
				}

				tGO.name = "pip";
				tSR = tGO.GetComponent<SpriteRenderer>();
				tSR.sprite = dictSuits[card.suit];
				tSR.sortingOrder = 1;
				card.pipGOs.Add (tGO);
			}

			//handle face cards
			if(card.def.face != ""){
				tGO = Instantiate(prefabSprite) as GameObject;
				tSR = tGO.GetComponent<SpriteRenderer>();
				tS = GetFace(card.def.face + card.suit);
				tSR.sprite = tS;
				tSR.sortingOrder = 1;
				tGO.transform.parent = card.transform;
				tGO.transform.localPosition = Vector3.zero;
					tGO.name = "face";
			}

			//add card back
			tGO = Instantiate(prefabSprite) as GameObject;
			tSR = tGO.GetComponent<SpriteRenderer>();
			tSR.sprite = cardBack;
			tGO.transform.parent = card.transform;
			tGO.transform.localPosition = Vector3.zero;

			tSR.sortingOrder = 2;
			tGO.name = "back";
			card.back = tGO;

			//default to face-up
			card.faceUp = false; //use the property faceup of card


								cards.Add (card);
						}
				}
	static public void Shuffle(ref List<Card> oCards){
				List<Card> tCards = new List<Card> ();

				int ndx;
				//repeat as long as there are cards in the original list
				while (oCards.Count > 0) {
						ndx = Random.Range (0, oCards.Count);
						tCards.Add (oCards [ndx]);
						oCards.RemoveAt (ndx);
				}
				//replace the original list with temp list
				oCards = tCards;
		}
	public Sprite GetFace (string faceS){
				foreach (Sprite tS in faceSprites) {
						if (tS.name == faceS) {
								return(tS);
						}
				}
				return (null);
		}

		


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
