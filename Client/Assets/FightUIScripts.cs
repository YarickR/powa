using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FightUIScripts : MonoBehaviour {

	public GameObject OpponentPanel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndTurnClick(int arg) {
		GCTX ctx = GCTX.Instance;
		if (ctx.User.EOTLevel != PConst.EOT_None) {
			ctx.User.EOTLevel = PConst.EOT_None;
		} else {
			ctx.User.EOTLevel = PConst.EOT_Hard;
			ctx.User.EOMTS = UnityEngine.Time.time;
		};
		ctx.Send_END_TURN();
		ctx.User.setEndTurnIndicator(ctx.User.EOTLevel);
	}

	public  void AttackClick() {
		GCTX ctx = GCTX.Instance;
		ctx.ToggleAttack(ctx.SelectedVehicle ? !ctx.SelectedVehicle.AttackMode : false);
	}

	public  void AIClick() {
		GCTX ctx = GCTX.Instance;
		ctx.ToggleAI();
	}
}
