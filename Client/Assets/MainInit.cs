using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class MainInit : MonoBehaviour {
	public HexTile hexTile;
	private float _lastEOMDTS;
	// Use this for initialization
	void Start () {
		GCTX ctx = GCTX.Instance;
		ctx.SelectedVehicle = null;
		ctx.MovingVehicle = null;
		ctx.Players = new List<PPlayer>();
	}

	// Update is called once per frame
	void Update () {
		return ;
		GCTX ctx = GCTX.Instance;
		if (ctx.MovingVehicle != null) {
			Vehicle __vh = ctx.MovingVehicle.GetComponent<Vehicle>();
			if ((__vh.Path.Count < 1) || (__vh.PathStep > __vh.Path.Count - 1)) {
				__vh.PathStep = 0;
				__vh.Path.Clear();
				__vh.LastMoveTS = 0;
				ctx.MovingVehicle = null;
				ctx.FieldCells[__vh.Y, __vh.X].GetComponent<SpriteRenderer>().color = Color.white;
				ctx.PassTheMove(ctx.CurrentMovePlayerId, true);
			} else 
				if ( Time.time - __vh.LastMoveTS >= 0.2) {
					PPoint __p = __vh.Path[__vh.PathStep];
					__vh.LastMoveTS = Time.time;
					ctx.FieldCells[__vh.Y, __vh.X].GetComponent<SpriteRenderer>().color = Color.white;
					__vh.PathStep++;
					__vh.Time--;
					__vh.MoveTo(__p.X, __p.Y);
					__vh.ShowVehicleInfo();
			};
		} else 
		if (ctx.ShootingVehicle != null) {
			//Nothing to do yet
		} else {
			PPlayer __p = ctx.Players[ctx.CurrentMovePlayerId];
			if (__p.EOMTS != 0) {
				if (__p.EOMTS >= UnityEngine.Time.time) {
					float __diff = __p.EOMTS - UnityEngine.Time.time;
					if (UnityEngine.Time.time - _lastEOMDTS > 0.2) {
						GameObject __EOMTimer =  GameObject.Find("Canvas/MoveTimer");
						__EOMTimer.transform.localScale = new Vector3(__diff > PConst.Move_Time ? 1 : __diff / PConst.Move_Time, 1, 1);
						_lastEOMDTS = UnityEngine.Time.time;
					}
				} else {
					if (__p.EOTLevel == 0) {	
						__p.EOTLevel = PConst.EOT_Soft;
					};
					bool __eot = true;
					foreach (PPlayer __pi in ctx.Players) {
						Debug.Log("Player " + __pi.PlayerId + ", EOTLevel: " + __pi.EOTLevel);
						__eot &= __pi.EOTLevel != 0;
					};
					if (__eot) {
						foreach (PPlayer __pi in ctx.Players) {
							__pi.EOTLevel = PConst.EOT_None;
							__pi.EOMTS = 0;
						};
						ctx.EndTurn();
					};
					ctx.PassTheMove(ctx.CurrentMovePlayerId, true);
				};
			}
		}
	}
}
