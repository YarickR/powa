using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Lobby : MonoBehaviour {
	void Start () {

	}
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateMatch() {
		GCTX ctx = GCTX.Instance;
		ctx.CreateMatch();
	}

	public void JoinMatch(int matchId) {
		GCTX ctx = GCTX.Instance;
		ctx.JoinMatch(matchId);
	}

	public void RefreshMatchList() {
		GCTX ctx = GCTX.Instance;
		ctx.RefreshMatchList();
	}
}