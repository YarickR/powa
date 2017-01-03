using UnityEngine;
using System.Collections;

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

	bool adjacentTo(int aX, int aY) {
		for (int __i = 0; __i < 6; __i++) {
			int __sX = Y & 1;
			__sX = __sX != 0 ? _adjShiftsOdd[__i].X : _adjShiftsEven[__i].X;
			int __sY = _adjShiftsOdd[__i].Y;
			if ((X + __sX  == aX) && (Y + __sY == aY)) {
				return true;
			}
		};
		return false;
	}
	void OnMouseEnter() {
		if (!Input.GetButton("Left") || Occupied || (Type == PConst.TType_Mountain)) {
			return;
		};

		GCTX ctx = GCTX.Instance;

		if (ctx.SelectedVehicle == null) {
			return;
		};
		Vehicle __vh = ctx.SelectedVehicle;
		if (__vh.PlayerId != ctx.User.GlobalId) {
			return;
		};
		int __aX = __vh.X;
		int __aY = __vh.Y;
		if (__vh.Path.Count > 0) {
			__aX = __vh.Path[__vh.Path.Count - 1].X;
			__aY = __vh.Path[__vh.Path.Count - 1].Y;
		};
		if (!adjacentTo(__aX, __aY)) {
			return;
		};
		if (__vh.Path.Count >= __vh.Time) {
			return;
		};
		if (__vh.Armor <= 0) {
			return;
		};
		int __i = __vh.FindPathPoint(X, Y);
		if (__i == -1) { // This is new point
			PPoint __p = new PPoint();
			__p.X = X;
			__p.Y = Y;
			__vh.Path.Add(__p);
		} else {
			__vh.RemovePathPoints(__i + 1);
		};
		GetComponent<SpriteRenderer>().color = new Color (1,1,1,0.2f);
	}

	void OnMouseExit() {
		
	}

	void OnMouseDown() {
		GCTX ctx = GCTX.Instance;


		if (ctx.SelectedVehicle == null) {
			return;
		};
		Vehicle __vh = ctx.SelectedVehicle;
		if (__vh.PlayerId != ctx.User.GlobalId) {
			Debug.Log("Not your vehicle");
			return;
		};
		if (!ctx.ActivityLock) {
			Debug.Log("Not your move");
			return;
		};

		if (__vh.Armor == 0) {
			Debug.Log("This vehicle is dead");
			return;
		};
		if (((__vh.Type == PConst.VType_Light) || (__vh.Type == PConst.VType_MediumRanged)) && __vh.AttackMode) {
			__vh.Shoot(X, Y, null, null);
			return;
		};
		bool __found = false;
		int __i;
		for (__i = 0; __i < __vh.Path.Count; __i++) {
			if (!__found && ((__vh.Path[__i].X == X) && (__vh.Path[__i].Y == Y))) {
				__found = true;	
				break;
			};
		};
		if (!__found) {
			return;
		};
		__vh.RemovePathPoints(__i + 1);
		ctx.MovingVehicle = ctx.SelectedVehicle;
		__vh.PathStep = 0;
		__vh.LastMoveTS = 0;
	}

	public void Highlight(bool hl, Color hlColor) {
		if ((!hl && !Highlighted) || (hl && Highlighted)) {
			return;
		};
		Highlighted = hl;
		GetComponent<SpriteRenderer>().color = hlColor;
	}
}
