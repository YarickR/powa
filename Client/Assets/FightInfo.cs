using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightInfo : MonoBehaviour {
	public int FightId { get; set ; }
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void JoinButtonClick() {
		GCTX ctx = GCTX.Instance;
		ctx.JoinFight(FightId);
	}
}
