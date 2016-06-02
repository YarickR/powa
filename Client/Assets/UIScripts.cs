using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScripts : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndTurnClick(int arg) {
		GCTX ctx = GCTX.Instance;
		ctx.Players[ctx.CurrentMovePlayerId].EOTLevel = PConst.EOT_Hard;
		ctx.Players[ctx.CurrentMovePlayerId].EOMTS = UnityEngine.Time.time;
	}

	public void AttackClick() {
		GCTX ctx = GCTX.Instance;
		ctx.ToggleAttack(ctx.SelectedVehicle ? !ctx.SelectedVehicle.GetComponent<Vehicle>().AttackMode : false);
	}
}
