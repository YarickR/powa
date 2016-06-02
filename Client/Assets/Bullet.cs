using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Bullet : MonoBehaviour {
	public int TargetX { get; set; }
	public int TargetY { get; set; }
	public Vehicle Target { get; set; }
	public int Type { get; set; }
	public int Damage { get; set; }
	public int Radius { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GCTX ctx = GCTX.Instance;
		Transform __t = this.gameObject.transform;
		int __x = Vehicle.hex2rectX(__t.position.x, __t.position.z);
		int __y = Vehicle.hex2rectY(__t.position.x, __t.position.z);
		if ((__x < 0) || (__y < 0) || (__x > ctx.FieldCells.GetUpperBound(1)) || (__y > ctx.FieldCells.GetUpperBound(0))) {
			Debug.Log("Position: " + __x + "x" + __y + ", target:" + TargetX + "x" +  TargetY + ", out of bounds");
			Destroy(this.gameObject);
			return;
		};
		if (__t.position.y < 0) {
			Debug.Log("Destroying due to ground hit - " + __x + ":" + __y + ", target was: " + TargetX + ":" + TargetY);
			Boom();
			Destroy(this.gameObject);
			return;
		};
		if (Type == PConst.BType_Cannon) {
			HexTile __tl = ctx.FieldCells[__y, __x];
			if (__tl.Type == PConst.TType_Mountain) {
				Debug.Log("Destroying due to mountain hit - " + __x + ":" + __y);
				Destroy(this.gameObject);
				return;
			};
		};
		if ((__x == TargetX) && (__y == TargetY)) {
			Boom();
			Destroy(this.gameObject);
			return;
		}
	}

	void Boom () {
		GCTX ctx = GCTX.Instance;
		if (Type == PConst.BType_Cannon) {
			Target.DamageVehicle(Damage);
		} else {
			for (int __p = 0; __p < ctx.Players.Count; __p++) {
				List<GameObject> __l = ctx.Players[__p].Units; 
				for (int __i = 0; __i < __l.Count; __i++) {
					Vehicle  __vhl = __l[__i].GetComponent<Vehicle>();
					int __dX = TargetX > __vhl.X ? TargetX - __vhl.X : __vhl.X - TargetX;
					int __dY = TargetY > __vhl.Y ? TargetY - __vhl.Y : __vhl.Y - TargetY;
					if ((__dX <= Radius) && (__dY <= Radius)) {
						__vhl.DamageVehicle(Math.Max(Radius - __dX, Radius - __dY));
					};
				};
			};
		};
	}

	void OnDestroy  () {
		GCTX ctx = GCTX.Instance;
		ctx.ShootingVehicle = null;
		ctx.PassTheMove(ctx.CurrentMovePlayerId, true);
	}
}
