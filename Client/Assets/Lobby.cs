using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Lobby : MonoBehaviour {
	void Start () {

	}
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateFight() {
		GCTX ctx = GCTX.Instance;
		ctx.CreateFight();
	}

	public void JoinFight(int fightId) {
		GCTX ctx = GCTX.Instance;
		ctx.JoinFight(fightId);
	}

	public void RefreshFightList() {
		GCTX ctx = GCTX.Instance;
		ctx.RefreshFightList();
	}
}