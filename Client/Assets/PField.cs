using System.Collections.Generic;
using UnityEngine;

public struct PPoint {
	public int X, Y;
	public static bool operator ==(PPoint a, PPoint b) { return (a.X == b.X) && (a.Y == b.Y);  }
	public static bool operator !=(PPoint a, PPoint b) { return (a.X != b.X) || (a.Y != b.Y);  }
}

public class PFPoint  {
	public int X, Y, prevX, prevY;
	public int cost;
	public int prio;
	public PFPoint(int newX, int newY) {
		X = newX;
		Y = newY;
		prevX = -1;
		prevY = -1;
		cost = -1;
		prio = 0;
	}
}

public class PField {
	public int Height { get; private set;  }
	public int Width { get; private set; }

	private HexTile[,] _cells ;
	public PField(int newWidth, int newHeight) {
		Height = newWidth;
		Width = newWidth;
		_cells = new HexTile[Height, Width];
	}

	public void Set(int x, int y, HexTile tile) {
		if ((x >= 0) || (x < Width) || (y >= 0) || (y < Height)) {
			_cells[y, x] = tile;
			return;
		};
		Debug.Log("Bad args " + x + "x" + y);
	}

	public HexTile Get(int x, int y) {
		if ((x >= 0) || (x < Width) || (y >= 0) || (y < Height)) {
			return _cells[y, x];
		};
		Debug.Log("Bad args " + x + "x" + y);
		return null;
	}

	public bool Occupied(int x, int y) {
		if ((x >= 0) || (x < Width) || (y >= 0) || (y < Height)) {
			return _cells[y,x].Occupied;
		};
		Debug.Log("Bad args " + x + "x" + y);
		return false;
	}

	public bool Higlighted(int x, int y) {
		if ((x >= 0) || (x < Width) || (y >= 0) || (y < Height)) {
			return _cells[y,x].Highlighted;
		};
		Debug.Log("Bad args " + x + "x" + y);
		return false;
	}

	public void Cleanup() {
		_cells = null;
	}

	public PPoint FindFreeCell(int xMin, int xMax, int yMin, int yMax) {
		PPoint ret;
		ret.X = -1; ret.Y = -1;
		int __counter = 100;
		do {
			int __y = UnityEngine.Random.Range(yMin, yMax + 1);
			if ((__y < 0) || (__y >= Height)) {
				continue;
			};
			int __x = UnityEngine.Random.Range(xMin, xMax + 1);
			if ((__x < 0) || (__x >= Width)) {
				continue;
			};
			if (Occupied(__x, __y)) {
				continue;
			};
			if (((_cells[__y, __x].Type >= PConst.TType_Desert) && 
				 (_cells[__y, __x].Type <= PConst.TType_Marsh)) ||
				 (_cells[__y, __x].Type == PConst.TType_Plain)) {
					ret.X = __x;
					ret.Y = __y;
					return ret;
			};
		} while (__counter-- > 0);
		return ret;
	}

	private static int MHDistance(int stX, int stY, int endX, int endY) {
		return Mathf.Abs(endX - stX) + Mathf.Abs(endY - stY);
	}

	public List<PPoint> FindPath(int stX, int stY, int endX, int endY) { // A* algo, priority queue done via list
		List<PFPoint> __f; // frontier
		PFPoint[,] __m;  // map copy
		int __x, __y;
		__f = new List<PFPoint>();
		__m = new PFPoint[Height, Width];
		for (__y = 0; __y < Height; __y++) {
			for (__x = 0; __x < Width; __x++) {
				__m[__y, __x] = new PFPoint(__x, __y);
			};
		};
		__m[stY, stX].cost = 0;
		__f.Add(__m[stY, stX]);
		PFPoint __curr = null ;
		bool found = false;
		while (__f.Count > 0) {
			__curr = __f[0]; // current cell
			__f.RemoveAt(0);
			if ((__curr.X == endX ) && (__curr.Y == endY)) {
				found = true;
				break;
			};
			List<PPoint> __ne = _cells[__curr.Y, __curr.X].Neighbours(); // neighbours
			while (__ne.Count > 0) {
				PPoint __nx = __ne[0]; // next neighbour
				__ne.RemoveAt(0);
				if ((__nx.X >= Width) || (__nx.Y >= Height)) {
					continue;
				};
				HexTile __t = _cells[__nx.Y, __nx.X]; // tile
				PFPoint __nxfp = __m[__nx.Y, __nx.X]; // next frontier point
				if (!__t.Occupied && 
					(((__t.Type >= PConst.TType_Desert) && (__t.Type <= PConst.TType_Marsh)) || (__t.Type == PConst.TType_Plain))) {
					int __c = __curr.cost + 1; // cost; 1 is a cost to move 
					if ((__nxfp.cost == -1) || (__c <  __nxfp.cost)) {
						__nxfp.cost = __c;
						__nxfp.prio = __c + MHDistance(__nx.X, __nx.Y, endX, endY); //priority
						int __i = 0;
						for (; __i < __f.Count; __i++) {
							if (__f[__i].prio >= __nxfp.prio) {
								break;	
							};
						};
						if (__i >= __f.Count) {
							__f.Add(__nxfp);
						} else {
							__f.Insert(__i, __nxfp);
						};
						__nxfp.prevX = __curr.X;
						__nxfp.prevY = __curr.Y;
					};
				};
			};	
		};
		List<PPoint> __ret = new List<PPoint>();
		if (found) {
			while ((__curr != null) && (__curr.prevX != -1)) {
				__ret.Add(new PPoint { X = __curr.X, Y = __curr.Y });
				__curr = __m[__curr.prevY, __curr.prevX];
			};
		};
		return __ret;
	}
}