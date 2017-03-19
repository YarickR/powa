using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MainInit : MonoBehaviour {
	public HexTile hexTile;

	// Use this for initialization
	void Start () {
		GCTX ctx = GCTX.Instance;
		ctx.SelectedVehicle = null;
		ctx.MovingVehicle = null;
		ctx.Players = new List<PPlayer>();
		ctx.ServerCommands = new List<JSONClass>();
		ctx.FightState = 0;
	}

	// Update is called once per frame
	void Update () {
		GCTX ctx = GCTX.Instance;
		ctx.Update();
	}

}
