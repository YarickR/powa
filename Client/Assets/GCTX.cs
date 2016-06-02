using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class GCTX : Singleton<GCTX> {
	protected GCTX () {} 

	public HexTile[,] FieldCells;
	public List<PPlayer> Players;
	public int UserId;
	public int CurrentMovePlayerId;
	public GameObject SelectedVehicle, MovingVehicle, ShootingVehicle;

	public PPoint FindFreeCell(int xMin, int xMax, int yMin, int yMax) {
		PPoint ret;
		ret.X = -1; ret.Y = -1;
		int __counter = 100;
		do {
			int __y = UnityEngine.Random.Range(yMin, yMax + 1);
			if ((__y < 0) || (__y >= FieldCells.GetLength(0))) {
				continue;
			};
			int __x = UnityEngine.Random.Range(xMin, xMax + 1);
			if ((__x < 0) || (__x >= FieldCells.GetLength(1))) {
				continue;
			};
			if (FieldCells[__y, __x].Occupied) {
				continue;
			};
			if (((FieldCells[__y, __x].Type >= PConst.TType_Desert) && 
				 (FieldCells[__y, __x].Type <= PConst.TType_Marsh)) ||
				 (FieldCells[__y, __x].Type == PConst.TType_Plain)) {
					ret.X = __x;
					ret.Y = __y;
					return ret;
			};
		} while (__counter-- > 0);
		return ret;
	}
	public void EndTurn() {
		int __i, __p;
		for (__p = 0; __p < Players.Count; __p++) {
			for (__i = 0; __i < Players[__p].Units.Count; __i++) {
				Vehicle __v = Players[__p].Units[__i].GetComponent<Vehicle>();
				__v.Time = __v.MaxTime;
				__v.AttackMode = false;
				if ((SelectedVehicle != null) && (Players[__p].Units[__i] == SelectedVehicle)) {
					__v.ShowVehicleInfo();
				} else {
					__v.ShowUnitStats();
				};
			};
		};
		ToggleAttack(false);
	}
	public void UnselectVehicle(GameObject vh) {
		Vehicle __vh = vh.GetComponent<Vehicle>();
		for (int __i = 0; __i < __vh.Path.Count ; __i++) {
			PPoint __pp = __vh.Path[__i];
			FieldCells[__pp.Y, __pp.X].GetComponent<SpriteRenderer>().color = Color.white;
		};
		__vh.Path.Clear();
		FieldCells[__vh.Y, __vh.X].GetComponent<SpriteRenderer>().color = Color.white;
		__vh.AttackMode = false;
		ToggleAttack(false);
	}

	public void SelectVehicle(GameObject vh) {
		Vehicle __vh = vh.GetComponent<Vehicle>();
		HexTile __t = FieldCells[__vh.Y, __vh.X];
		__t.GetComponent<SpriteRenderer>().color = new Color (1,1,1,0.2f);
		__vh.ShowVehicleInfo();
		__vh.AttackMode = false;
		ToggleAttack(false);
	}

	public void ToggleAttack(bool on) {
		if (SelectedVehicle == null) {
			return;
		}
		Vehicle __vh = SelectedVehicle.GetComponent<Vehicle>();
		GameObject __o = GameObject.Find("Canvas/AttackButton");
		Image __btnImg = __o.GetComponent<Image>();
		if (!on) {
			__vh.AttackMode = false;
			__btnImg.color = Color.white;
			for (int __y = 0; __y < FieldCells.GetLength(0); __y++) {
				for (int __x = 0; __x < FieldCells.GetLength(1); __x++) {
					FieldCells[__y, __x].Highlight(false, Color.white);
				};
			};
		} else {
			if ((__vh.Armor == 0) || (__vh.Time < __vh.ShotTU) || (__vh.PlayerId != CurrentMovePlayerId)) {
				return;
			};
			__vh.AttackMode = true;
			__btnImg.color = Color.red;
			for (int __y = 0; __y < FieldCells.GetLength(0); __y++) {
				for (int __x = 0; __x < FieldCells.GetLength(1); __x++) {
					int __dist = Mathf.RoundToInt(Mathf.Sqrt(Math.Abs(__y - __vh.Y) * Math.Abs(__y - __vh.Y) + Math.Abs(__x - __vh.X) * Math.Abs(__x - __vh.X)));
					if (__dist <= __vh.Distance) {
						FieldCells[__y, __x].Highlight(true, new Color(1,1,1,0.5f));
					};
				};
			};

		};
	}

	public GameObject FindVehicleByPos(int x, int y, List<GameObject> list) {
		for (int __i = 0; __i < list.Count; __i++) {
			Vehicle __vh = list[__i].GetComponent<Vehicle>();
			if ((__vh.X == x) && (__vh.Y == y)) {
				return list[__i];
			}
		};
		return null;
	}

	public void PassTheMove(int PlayerId, bool next = true) {
		if (next) {
			PlayerId = (PlayerId + 1) % Players.Count;
		};
		Players[PlayerId].EOMTS = UnityEngine.Time.time + PConst.Move_Time + 1; // One more second of full timer
		CurrentMovePlayerId = PlayerId;
		GameObject __img = GameObject.Find("Canvas/MoveTimer");
		__img.GetComponent<Image>().color = Players[PlayerId].BaseColor;
	}
}
