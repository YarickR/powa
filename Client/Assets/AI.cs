using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI {
	
	private AIStep _aiStep;
	private IEnumerator<int> _aiStepIE;

	public AI() {
		_aiStep = new AIStep();
		_aiStepIE = _aiStep.GetEnumerator();
	}

	public void Step() {
		GCTX.Log("AI Cycle step");
		if (_aiStepIE.MoveNext() == false) {
			_aiStepIE = _aiStep.GetEnumerator();
		};
	}
}

public class AIStep : IEnumerable<int> {
	private int[,] _heatMap;
	public AIStep() {
	}
	public void Reset() {
		_heatMap = null;
	}

	IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

	public IEnumerator<int> GetEnumerator() {
		GCTX.Log("AI Cycle start");

		GCTX ctx = GCTX.Instance;
		int __av = 0;
		List<Vehicle>.Enumerator __u = ctx.User.Units.GetEnumerator();
		while (__u.MoveNext() != false) { 
			GCTX.Log("Checking " + __u.Current.ToString() + ", Time: " + __u.Current.Time + ", Armor: " + __u.Current.Armor);
			if ((__u.Current.Time > 0) && (__u.Current.Armor > 0)) {
				ctx.SelectedVehicle = __u.Current;
				__av++;
				List<PPoint> __hps;
				yield return __av;
				
				Vehicle __tv = __u.Current.Time >= __u.Current.ShotTU ? __u.Current.FindTarget() : null; // target vehicle
				if (__tv == null) {
					__hps = GenerateHM(); 
					PPoint __ep = new PPoint(0, 0); // end point 
					while (__hps.Count > 0) {
						yield return __av;
						int __idx = Random.Range(1, (__u.Current.Armor / __u.Current.MaxArmor) * 100) > 15 ? 
								0 : __hps.Count / 2; // Advancing if healthy, more probably retreating otherwise
						__ep = __hps[__idx]; 
						__hps.RemoveAt(__idx);
						GCTX.Log("Checking point #" + __idx + " " + __ep.X + "x" + __ep.Y);
						List<PPoint> __p = ctx.Field.FindPath(__u.Current.X, __u.Current.Y, __ep.X, __ep.Y);
						if (__p.Count > 0) {
							break;
						};
						__ep.X = -1;
						__ep.Y = -1;
					};
					if (__ep.X >= 0) {
						HexTile __hp = ctx.Field.Get(__ep.X, __ep.Y);
						yield return __av;
						__hp.OnMouseDown();
						yield return __av; //  Waiting about 1/10 second , each Update is run 30 times per second
						yield return __av;
						yield return __av;
						__hp.OnMouseDown(); // Start moving
						yield return __av;
					} else {
						GCTX.Log("Was unable to find a path from this vehicle to another point on the map");
						__av--;
					};
				} else {
					__u.Current.Shoot(__tv.X, __tv.Y, __tv, null);
				};
			};
			GCTX.Log("__av: " + __av);
		};
		if (__av == 0) {
			GCTX.Log("No more units to check, ending turn");
			ctx.FightUI.GetComponent<FightUIScripts>().EndTurnClick(1);
		};
		GCTX.Log("AI Cycle end");
		yield break;
	}

	private List<PPoint> GenerateHM() {
		List<PPoint> __ret = new List<PPoint>() ;
		GCTX ctx = GCTX.Instance;
		_heatMap = new int[ctx.Field.Height,ctx.Field.Width];
		int __maxD = Mathf.RoundToInt(Mathf.Sqrt(ctx.Field.Height * ctx.Field.Height  + ctx.Field.Width * ctx.Field.Width));

		/*  We're building a heat map, enemy units add up heat, 
			my units cool cells up (so we're choosing places close to enemy but possibly farther away 
			from our troops */
		for (int __y = 0; __y < ctx.Field.Height; __y++) {
			for (int __x = 0; __x < ctx.Field.Width; __x++) {
				_heatMap[__y, __x] = 0;	
				if (!ctx.Field.Occupied(__x, __y)) {
					foreach (PPlayer __p in ctx.Players) {
						foreach (Vehicle __v in __p.Units) {
							if (__v.Armor == 0) { 
								// disabled vehicles don't influence decisions
								continue;
							};
							int __dist = __maxD - Mathf.RoundToInt(Mathf.Sqrt((__v.X - __x) * (__v.X - __x) +  (__v.Y - __y) * (__v.Y - __y)));
							_heatMap[__y, __x] = __p.GlobalId == ctx.User.GlobalId ? _heatMap[__y, __x] - (__dist / 2) : _heatMap[__y, __x] + __dist;
						};
						__ret.Add(new PPoint { X = __x, Y = __y } );
					};
				};
			};
		};
		__ret.Sort(delegate(PPoint a, PPoint b) {
			if (_heatMap[a.Y, a.X] < _heatMap[b.Y, b.X]) return -1;
			if (_heatMap[a.Y, a.X] > _heatMap[b.Y, b.X]) return 1;
			return 0;
		});
		__ret.Reverse();
		return __ret;
	}
}

