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
	public List<PDamage> DamageList;
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
			GCTX.Log("Position: " + __x + "x" + __y + ", target:" + TargetX + "x" +  TargetY + ", out of bounds");
			Destroy(this.gameObject);
			return;
		};
		if (__t.position.y < 0) {
			GCTX.Log("Destroying due to ground hit - " + __x + ":" + __y + ", target was: " + TargetX + ":" + TargetY);
			Boom();
			Destroy(this.gameObject);
			return;
		};
		if (Type == PConst.BType_Cannon) {
			HexTile __tl = ctx.Field.Get(__x, __y);
			if (__tl.Type != PConst.TType_Ground) {
				GCTX.Log("Destroying due to obstacle hit - " + __x + ":" + __y);
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
		DamageList.ForEach(delegate (PDamage d) { 
			d.Vhcl.DamageVehicle(d.Damage, true);
		});
	}

	void OnDestroy  () {
		GCTX ctx = GCTX.Instance;
		ctx.ShootingVehicle = null;
		if (ShootCommand != null) {
			ShootCommand ["state"].AsInt = PCmdState.DONE;
		};
	}
}
