using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CardState{
	drawpile,
	tableau,
	target,
	discard
}

public class CardProspector : Card {
	public CardState state = CardState.drawpile;
	public List<CardProspector> hiddenBy = new List<CardProspector>();
	public int layoutID;
	public SlotDef slotDef;

	// Use this for initialization
	void Start () {
	
	}
	
	override public void OnMouseUpAsButton(){
				Prospector.S.CardClicked (this);
				base.OnMouseUpAsButton ();
		}

}
