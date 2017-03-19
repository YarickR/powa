using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTile : MonoBehaviour {
	public int X { get; set; }
	public int Y { get; set; }
	public int Type { get; set; }
	public bool Occupied { get; set; }
	public bool Highlighted { get; set; }

	static PPoint[] _adjShiftsEven = new PPoint[] { 
		new PPoint() {X = -1,  Y = 1}, 
		new PPoint() {X = 0,  Y = 1}, 
		new PPoint() {X = -1, Y = 0}, 
		new PPoint() {X = 1,  Y = 0}, 
		new PPoint() {X = -1,  Y = -1}, 
		new PPoint() {X = 0,  Y = -1}
	};
	static PPoint[] _adjShiftsOdd = new PPoint[] { 
		new PPoint() {X = 0,  Y = 1}, 
		new PPoint() {X = 1,  Y = 1}, 
		new PPoint() {X = -1, Y = 0}, 
		new PPoint() {X = 1,  Y = 0}, 
		new PPoint() {X = 0,  Y = -1}, 
		new PPoint() {X = 1,  Y = -1}
	};
	// Use this for initialization
	void Start () {
		Occupied = false;
		Highlighted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override  string ToString() {
		return System.String.Format("C({0}x{1})", X, Y);
	}

	public bool adjacentTo(int aX, int aY) {
		for (int __i = 0; __i < 6; __i++) {
			int __sX, __sY;
			__sX = (Y & 1) != 0 ? _adjShiftsOdd[__i].X : _adjShiftsEven[__i].X;
			__sY = _adjShiftsOdd[__i].Y;
			if ((X + __sX  == aX) && (Y + __sY == aY)) {
				return true;
			}
		};
		return false;
	}

	public List<PPoint> Neighbours() {
		List<PPoint> __ret = new List<PPoint>(6); // no more than 6 neighbours for hex tile
		for (int __i = 0; __i < 6; __i++) {
			int __sX, __sY;
			__sX = (Y & 1) != 0 ? _adjShiftsOdd[__i].X : _adjShiftsEven[__i].X;
			__sY = _adjShiftsOdd[__i].Y;
			if (((X + __sX) >= 0) && ((Y + __sY) >= 0)) {
				__ret.Add(new PPoint { X = X + __sX, Y = Y + __sY });
			};
		};
		return __ret;
	}

	void OnMouseEnter() {

	}

	public void OnMouseExit() {
		
	}

	public void OnMouseDown() {
		GCTX ctx = GCTX.Instance;


		if (ctx.SelectedVehicle == null) {
			GCTX.Log(ToString() + " - no selected vehicle");
			return;
		};
		Vehicle __vh = ctx.SelectedVehicle;
		if (__vh.PlayerId != ctx.User.GlobalId) {
			GCTX.Log(ToString() + " - not your vehicle");
			return;
		};
		if (!ctx.ActivityLock) {
			GCTX.Log(ToString() + " - not your move");
			return;
		};

		if (__vh.Armor == 0) {
			GCTX.Log(ToString() + " - this vehicle is dead");
			return;
		};
		if (((__vh.Type == PConst.VType_Light) || (__vh.Type == PConst.VType_MediumRanged)) && __vh.AttackMode) {
			GCTX.Log(ToString() + " - shooting AOE");
			__vh.Shoot(X, Y, null, null);
			return;
		};
		/* Ok we need to either show a path or start movement */
		List<PPoint> __newPath = ctx.Field.FindPath(__vh.X, __vh.Y, X, Y);
		if (__newPath.Count > 0) {
			if ((__newPath.Count == __vh.Path.Count) && 
				(__newPath[0] == __vh.Path[__vh.Path.Count - 1])) { // We're getting last to first list of points from A* algo 
				GCTX.Log(ToString() + "- preparing movement");
				ctx.MovingVehicle = ctx.SelectedVehicle;
				__vh.PathStep = 0;
				__vh.LastMoveTS = 0;
				int __pcc = __vh.Time;
				bool __pe;
				__newPath.Clear();
				List<PPoint>.Enumerator __p = __vh.Path.GetEnumerator();
				__pe = __p.MoveNext();
				while ((__pcc > 0) && __pe ) {
					__pcc--; // Actually we need to decrease path cost counter according to the cost of moving between those path points. 
					__newPath.Add(__p.Current);
					__pe = __p.MoveNext();
				};
				if (__pe) {
					__vh.HidePath(__p);
				};
				__vh.Path = __newPath;
			} else {
				GCTX.Log(ToString() + " - displaying path");
				__vh.HidePath();
				__newPath.Reverse();
				__vh.Path = __newPath;
				__vh.ShowPath(__vh.Time);
			};
		} else {
			GCTX.Log(ToString() + " - was unable to find a path from " + ctx.SelectedVehicle.ToString());
		}
	}

	public void Highlight(bool hl, Color hlColor) {
		if ((!hl && !Highlighted) || (hl && Highlighted)) {
			return;
		};
		Highlighted = hl;
		GetComponent<SpriteRenderer>().color = hlColor;
	}

	public bool Passable() {
		return (((Type >= PConst.TType_Desert) && (Type <= PConst.TType_Marsh)) || 
				(Type == PConst.TType_Plain));
	}
}
