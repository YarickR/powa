using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchRow : MonoBehaviour {
	public int MatchId { get; set ; }
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void JoinButtonClick() {
		GameObject __o =  GameObject.Find("Canvas/Lobby");
		Lobby __l = __o.GetComponent<Lobby>();
		__l.JoinMatch(MatchId);
	}
}
