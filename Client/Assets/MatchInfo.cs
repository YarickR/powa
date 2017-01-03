using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchInfo : MonoBehaviour {
	public int MatchId { get; set ; }
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void JoinButtonClick() {
		GCTX ctx = GCTX.Instance;
		ctx.JoinMatch(MatchId);
	}
}
