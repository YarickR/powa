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
		if (ctx.FieldCells == null) {
			ctx.FieldCells = new HexTile[PConst.Map_Size, PConst.Map_Size];
		};
		ctx.SelectedVehicle = null;
		ctx.MovingVehicle = null;

		Sprite[] __hexTiles = Resources.LoadAll<Sprite>("Terrain");
		for (int __h = 0; __h < PConst.Map_Size; __h++) {
			for (int __w = 0; __w < PConst.Map_Size; __w++) {
				HexTile __t;
				Vector3 __v = new Vector3(Vehicle.rect2hexX(__w, __h), (float)(__h * 0.01), Vehicle.rect2hexY(__w, __h));
				__t = ( HexTile)Instantiate(hexTile,  __v, Quaternion.Euler(90, 0, 0));

				if (UnityEngine.Random.Range (0, 100) < 30) {
					__t.Type = UnityEngine.Random.Range (PConst.TType_Desert, PConst.TType_Marsh + 1);
				} else
					if (UnityEngine.Random.Range (0, 100) < 20) {
					__t.Type = PConst.TType_Mountain;
				} else {
					__t.Type = PConst.TType_Plain;
				};
				__t.GetComponent<SpriteRenderer> ().sprite = __hexTiles [(__t.Type * 4) + UnityEngine.Random.Range(0, 4)];
				__t.X = __w;
				__t.Y = __h;
				ctx.FieldCells [__h, __w] = __t;
			};
		};
		ctx.Players = new List<PPlayer>();
		ctx.UserId = UnityEngine.Random.Range(0, PConst.Max_Players);
		for (int __i = 0; __i < PConst.Max_Players; __i++) {
			ctx.Players.Add(new PPlayer (50, __i, __i == ctx.UserId ? Color.blue : Color.red ));
		};

		string[] __prefabNames = new string[5] {"", "Vehicle_Light", "Vehicle_LightRanged", "Vehicle_Medium", "Vehicle_MediumRanged"};
		int [,] __corners = new int[4,4] { 	{1, 4, 1, 4}, 
											{PConst.Map_Size - 5, PConst.Map_Size -1, PConst.Map_Size - 5, PConst.Map_Size -1}, 
											{PConst.Map_Size - 5, PConst.Map_Size -1, 1, 4}, 
											{ 1, 4, PConst.Map_Size - 5, PConst.Map_Size -1}};
		foreach (PPlayer __p in ctx.Players) {
			for (int __i = PConst.VType_Light ; __i <= PConst.VType_MediumRanged ; __i++) {
				string __pfName = __p.PlayerId == ctx.UserId ? __prefabNames[__i] + "Blue" : __prefabNames[__i] + "Red"; 
				GameObject __prefab = Resources.Load<GameObject>(__pfName);
				GameObject __vh = (GameObject)Instantiate(__prefab, new Vector3(0, 0, 0), Quaternion.identity); 
				Vehicle __vhl = __vh.GetComponent<Vehicle>();
				__vhl.Type = __i;
				__vhl.PlayerId = __p.PlayerId;
				__vhl.Id = __p.Units.Count;
				__p.Units.Add(__vh);

				PPoint __freeCell;
				__freeCell = ctx.FindFreeCell(	__corners[ __p.PlayerId, 0], 
												__corners[ __p.PlayerId, 1], 
												__corners[ __p.PlayerId, 2], 
												__corners[ __p.PlayerId, 3]);
				if (__freeCell.X == -1) {
					break;
				};
				__vhl.MoveTo(__freeCell.X, __freeCell.Y);
			};
		};
		ctx.PassTheMove(0, false);
	}

	// Update is called once per frame
	void Update () {
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
