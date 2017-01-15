using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Bullet : MonoBehaviour {
	public int TargetX { get; set; }
	public int TargetY { get; set; }
	public Vehicle Shooter { get; set; }
	public Vehicle Target { get; set; }
	public JSONClass ShootCommand;
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
		if ((__x < 0) || (__y < 0) || (__x >= ctx.Field.Width) || (__y > ctx.Field.Height)) {
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
			HexTile __tl = ctx.Field.Get(__x, __y);
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
			ctx.Players.ForEach(delegate(PPlayer p) {
				p.Units.ForEach(delegate(Vehicle v) {
					int __dX = TargetX > v.X ? TargetX - v.X : v.X - TargetX;
					int __dY = TargetY > v.Y ? TargetY - v.Y : v.Y - TargetY;
					if ((__dX <= Radius) && (__dY <= Radius)) {
						v.DamageVehicle(Math.Max(Radius - __dX, Radius - __dY));
					};	
				});
			});
		};
	}

	void OnDestroy  () {
		GCTX ctx = GCTX.Instance;
		ctx.ShootingVehicle = null;
		if (ShootCommand != null) {
			ShootCommand ["state"].AsInt = PCmdState.DONE;
		};
	}
}
