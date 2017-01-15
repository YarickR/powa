using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class Vehicle : MonoBehaviour {
	public GameObject Projectile;
	public GameObject OVHStats;
	public GameObject UnitStats;
	public int MaxTime { get; private set; }
	public int MaxDamage { get; private set; }
	public int MaxArmor { get; private set; }
	public int MaxDistance { get; private set; }
	public int Time { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }
	public int ShotTU { get; set; }
	public int Distance { get; set; }
	public int Price { get; private set; }
	public int X { get; set; }
	public int Y { get; set; }
	public int Type { get; set; }
	public int PlayerId { get; set; }
	public int Id { get; set; }
	public List<PPoint> Path;
	public int PathStep { get; set; }
	public float LastMoveTS { get; set; }
	public bool AttackMode { get; set; }
	public JSONClass MoveCommand;
	private Material[] _allMaterials;
	private Color[] _matEmiColors;
	private GameObject _ovh; // overhead stats panel
	private OVHStats _ovhs; // overhead stats panel script
	private GameObject _usp; // unit stats panel
	private UnitStats _usps; // unit stats panel script
	private Camera _eye;
	private float _flashStopTS;
	private Collider _collider;
	private RectTransform _ovht; // UnitStats panel transform
	private Renderer _renderer;
	private Vector3[] _bbCorners; // bounding box corners 
	private float[,] _mmXYZ; // min,max x,y,z

	public Material[] Materials { get { return _allMaterials; } }
	public static float rect2hexX(int x, int y) {
		return (float)( x * 2.56 + (y & 1) * 1.28);
	}
	public static float rect2hexY(int x, int y) {
		return (float) (y * 1.92);
	}

	public static int hex2rectX(float x, float y) {
		int __y = (int)Math.Round((y - 0.96) / 1.92);
		return (__y & 1) != 0 ? (int)Math.Round((x - 1.28) / 2.56) : (int)Math.Round(x / 2.56);
	}
	public static int hex2rectY(float x, float y) {
		return  (int)Math.Round(y / 1.92);
	}

	// Use this for initialization
	void Start () {
		
		switch (Type) {
			case PConst.VType_Light: MaxTime= 10;	MaxDamage = 2; MaxArmor = 4; Price = 2;	ShotTU = 2; MaxDistance = 4; break;
			case PConst.VType_LightRanged: MaxTime = 10;	MaxDamage = 2; MaxArmor = 4; Price = 2;	ShotTU = 2; MaxDistance = 4; break;
			case PConst.VType_Medium: MaxTime = 5; MaxDamage = 4; MaxArmor = 8; Price = 5; ShotTU = 3; MaxDistance = 6; break;
			case PConst.VType_MediumRanged: MaxTime = 5; MaxDamage = 8; MaxArmor = 10; Price = 8; ShotTU = 3; MaxDistance = 6; break;
		};
		Time = MaxTime;
		Damage = MaxDamage;
		Armor = MaxArmor;
		Distance = MaxDistance;
		Path = new List<PPoint>();
		AttackMode = false;
		MeshRenderer[] __meshes = GetComponentsInChildren<MeshRenderer>();
		_allMaterials = new Material[__meshes.GetLength(0)];
		_matEmiColors = new Color[__meshes.GetLength(0)];
		int __i = 0;
		foreach (MeshRenderer __m in __meshes) {
			_allMaterials[__i] = __m.material;
			_allMaterials[__i].EnableKeyword("_EMISSION");
			_matEmiColors[__i] = _allMaterials[__i].GetColor("_EmissionColor");
			_matEmiColors[__i].r *= 2.5f;
			_matEmiColors[__i].g *= 2.5f;
			_matEmiColors[__i].b *= 2.5f;
			__i++;
		};
		_flashStopTS = 0;
		Flash(0.5f, new Color(0.7f, 0.7f, 0.7f, 0.7f));
		GCTX ctx = GCTX.Instance;
		GameObject __c = GameObject.Find("EyeSocket/Eye");
		_eye = __c.GetComponent<Camera>();
		if (PlayerId == ctx.User.GlobalId) {
			_ovh = Instantiate(OVHStats) as GameObject;
			GameObject __fui = ctx.FightUI ;
			_ovh.transform.SetParent(GameObject.Find("UI").transform, false);
			_ovht = _ovh.GetComponent<RectTransform>();
			_ovhs = _ovh.GetComponent<OVHStats>();
			_ovhs.Cam = _eye;
			GameObject __up = __fui.transform.Find("UnitsPanel").gameObject;
			_usp = Instantiate(UnitStats) as GameObject;
			_usp.transform.SetParent(__up.transform, false);
			_usps = _usp.GetComponent<UnitStats>();
			_usps.UnitPic.sprite = GetComponent<SpriteRenderer>().sprite;
			_renderer = GetComponent<Renderer>();
			if (_renderer == null) {
				Debug.Log("No renderer for this object");
			} else {
				_bbCorners = new Vector3[8];
				for (__i = 0 ; __i < 8 ; __i++) {
					_bbCorners[__i] = new Vector3(0, 0, 0);
				};
				_mmXYZ = new float[2,3];
			}
		} else {
			_ovh = null;
			_usp = null;
		};
		_collider = GetComponent<Collider>();
		ShowUnitStats();
	}

	Vector3 GetHUDPoint() {
		float __minX, __minY, __maxX, __maxY;
		_mmXYZ[0, 0] = Mathf.Min(_renderer.bounds.min.x, _renderer.bounds.max.x);
		_mmXYZ[0, 1] = Mathf.Min(_renderer.bounds.min.y, _renderer.bounds.max.y);
		_mmXYZ[0, 2] = Mathf.Min(_renderer.bounds.min.z, _renderer.bounds.max.z);
		_mmXYZ[1, 0] = Mathf.Max(_renderer.bounds.min.x, _renderer.bounds.max.x);
		_mmXYZ[1, 1] = Mathf.Max(_renderer.bounds.min.y, _renderer.bounds.max.y);
		_mmXYZ[1, 2] = Mathf.Max(_renderer.bounds.min.z, _renderer.bounds.max.z);
		__minX = -1;
		__minY = -1;
		__maxX = -1;
		__maxY = -1;
		Vector3 __tempVec = new Vector3(0, 0, 0);
		Vector3 __w2s = new Vector3(0, 0, 0);
		Vector3 __ret = new Vector3(0, 0, 0);
		for (int __x = 0; __x <= 1 ; __x++) {
			for (int __y = 0; __y <= 1; __y++) {
				for (int __z = 0; __z <= 1; __z++) {
					__tempVec.x = _mmXYZ[__x, 0];
					__tempVec.y = _mmXYZ[__y, 1];
					__tempVec.z = _mmXYZ[__z, 2];
					__w2s = Camera.main.WorldToScreenPoint(__tempVec);
					__minX = __minX == -1 ? __w2s.x : Mathf.Min(__minX, __w2s.x);
					__maxX = __maxX == -1 ? __w2s.x : Mathf.Max(__maxX, __w2s.x); 
					__minY = __minY == -1 ? __w2s.y : Mathf.Min(__minY, __w2s.y);
					__maxY = __maxY == -1 ? __w2s.y : Mathf.Max(__maxY, __w2s.y); 
				};
			};
		};
		__ret.x =  __minX + ((__maxX - __minX) / 2);
		__ret.y =  __maxY - ((__maxY - __minY) / 4);
		__ret.z =  __w2s.z;
		return __ret;
	}
	// Update is called once per frame
	void Update () {
		if (_ovh) {
	    	_ovh.transform.position = GetHUDPoint();
		};
		if (_flashStopTS != 0) {
			if ( _flashStopTS < UnityEngine.Time.time ) {
				for (int __i = 0; __i < _allMaterials.GetLength(0); __i++) {
					_allMaterials[__i].SetColor("_EmissionColor", _matEmiColors[__i]);
				};
				_flashStopTS = 0;
			};
		};
	}

	public void Cleanup() {
		if (_ovh) {
			GameObject.Destroy(_ovh);
			_ovh = null;
		}
		if (_usp) {
			GameObject.Destroy(_usp);
			_usp = null;
		};
	}
	public void Flash(float delay, Color newColor) {
		for (int __i = 0; __i < _allMaterials.GetLength(0); __i++) {
			_allMaterials[__i].SetColor("_EmissionColor", newColor);
		};
		_flashStopTS = UnityEngine.Time.time + delay;
	}

	public void MoveTo(int newX, int newY) {

		GCTX ctx = GCTX.Instance;
		ctx.Field.Get(X, Y).Occupied = false;
		transform.position = new Vector3(rect2hexX(newX, newY), (float)0.01, rect2hexY(newX, newY));
		transform.rotation = Quaternion.identity;
		ctx.Field.Get(newX, newY).Occupied = true;
		X = newX;
		Y = newY;
	}

	public void FinishMove() {
		PathStep = 0;
		Path.Clear();
		LastMoveTS = 0;
	}

	public void MoveStep() {
		GCTX ctx = GCTX.Instance;
		PPoint __p = Path[PathStep];
		LastMoveTS = UnityEngine.Time.time;
		ctx.Field.Get(X, Y).GetComponent<SpriteRenderer>().color = Color.white;
		PathStep++;
		Time--;
		MoveTo(__p.X, __p.Y);
		if (PlayerId == ctx.User.GlobalId) {
			ShowVehicleInfo();
		};
	}

	void OnMouseDown() {
		GCTX ctx = GCTX.Instance;
		if (ctx.SelectedVehicle != null) {
			if (ctx.SelectedVehicle.AttackMode && (ctx.SelectedVehicle.PlayerId != PlayerId)) {
				ctx.SelectedVehicle.Shoot(X, Y, this, null);
				return;
			};
		};
		if (PlayerId == ctx.User.GlobalId) {
			if (ctx.SelectedVehicle && ctx.SelectedVehicle.Id != Id) {
				ctx.UnselectVehicle(ctx.SelectedVehicle);
			};
			ctx.SelectedVehicle = this;
			ctx.SelectVehicle(this);
		};
	}

	void OnMouseUp() {
		
	}

	public bool Shoot(int tgtX, int tgtY, Vehicle vh, JSONClass cmd) {
		GCTX ctx = GCTX.Instance;
		Debug.Log("Shoot");
		if ((Armor <= 0) || (Time < ShotTU) || !ctx.HisMoveNow(PlayerId)) {
			return false;
		};
		if ((Type == PConst.VType_LightRanged) || (Type == PConst.VType_Medium)) {
			if (vh == null) {
				Debug.Log("Shooting without target");
				return false;
			};
			tgtX = vh.X;
			tgtY = vh.Y;
		};
		float __dist = Mathf.Ceil(Mathf.Sqrt((X - tgtX) * (X - tgtX) + (Y - tgtY) * ( Y - tgtY)));
		if (__dist > Distance) {
			Debug.Log("Not in range");
			return false;
		};
		Vector3 __v = new Vector3(rect2hexX(X, Y), 1, rect2hexY(X, Y));
		Vector3 __t = new Vector3(rect2hexX(tgtX, tgtY), 1, rect2hexY(tgtX, tgtY));
		GameObject __proj = (GameObject)Instantiate(Projectile, __v, Quaternion.identity);
		Bullet __b = __proj.GetComponent<Bullet>();
		__b.Shooter = this;
		__b.ShootCommand = cmd;
		__b.TargetX = tgtX;
		__b.TargetY = tgtY;
		__b.Target = vh;
		switch (Type) {
			case PConst.VType_Light: 		__b.Damage = 2; __b.Radius = 1; break;
			case PConst.VType_LightRanged: 	__b.Damage = 2; __b.Radius = 1; break;
			case PConst.VType_Medium: 		__b.Damage = 4; __b.Radius = 2; break;
			case PConst.VType_MediumRanged: __b.Damage = 4; __b.Radius = 2; break;
		};
		Rigidbody __rb = __proj.GetComponent<Rigidbody>();
		__t = __t - __v;
		float __f = __t.magnitude;
		__t = Vector3.Normalize(__t);

		if ((Type == PConst.VType_Light) || (Type == PConst.VType_MediumRanged)) {
			__t.y = 0.65f;
			__f *= 2.1f;
			__b.Type = PConst.BType_Howitzer;
		} else {
			__f *= 1;						
			__b.Type = PConst.BType_Cannon;
		};
		__rb.AddForce(__t * __f, ForceMode.Impulse);
		Flash(0.1f, new Color(0.5f,0.5f,0.5f,0.2f));
		Time -= ShotTU;
		if (PlayerId == ctx.User.GlobalId) {
			ctx.Send_VEHICLE_ATTACK(this, vh, tgtX, tgtY);
		};
		if (ctx.SelectedVehicle.Id == Id) {
			ShowVehicleInfo();
		};
		AttackMode = false;
		ctx.ToggleAttack(false);
		ctx.ShootingVehicle = this;
		Debug.Log("Fired");
		return true;
	}

	public void DamageVehicle(int dmg) {
		if (dmg <= 0) {
			return;
		};
		GCTX ctx = GCTX.Instance;
		Armor = Math.Max(0, Armor - dmg);
		Debug.Log("Dealing " + dmg + " units of damage");
		if (Armor == 0) {
			for (int __i = 0; __i < _matEmiColors.GetLength(0); __i++) {
				_matEmiColors[__i].r /= 3;
				_matEmiColors[__i].g /= 3;
				_matEmiColors[__i].b /= 3;
			};
			Time = 0;
		};
		Flash(0.1f, new Color(0.5f,0,0,0.2f));
		ShowUnitStats();
	}

	void OnGUI() {
		GCTX ctx = GCTX.Instance;
		if (PlayerId == ctx.UserId) {
			Vector3 __s = _ovht.transform.localScale;
			__s.x  = 14 / _eye.fieldOfView;
			_ovht.transform.localScale = __s;
			Bounds __b = _collider.bounds;
			Vector3 __min = _eye.WorldToScreenPoint(__b.min);
			Vector3 __max = _eye.WorldToScreenPoint(__b.max);
			Vector3 __ctr = _eye.WorldToScreenPoint(__b.center);
			Vector3 __usSC = new Vector3(__ctr.x , __max.y, __min.z); 

			__usSC.x -= (_ovht.rect.width * __s.x) / 2;
			_ovh.transform.position = __usSC;
		};
	}

	public void ShowVehicleInfo() {
		ShowUnitStats();
	}

	public void ShowUnitStats() {
		if (_ovh != null) {
			_ovhs.TUS.value = Armor > 0 ? Mathf.Round((Time * 100)/MaxTime) : 0;
			_ovhs.Armor.value = Armor > 0 ? Mathf.Round((Armor * 100)/MaxArmor) : 0;
		};
		if (_usp != null) {
			_usps.TimerStats.text = Armor > 0 ? Time + "/" + MaxTime : "0";
			_usps.ArmorStats.text = Armor > 0 ? Armor + "/" + MaxArmor : "0";
			_usps.AttackStats.text =  MaxDamage + "/" + ShotTU + "/" + MaxDistance ;
			_usps.UnitPic.sprite = GetComponent<SpriteRenderer>().sprite;
		};
	}

	public void NextTurn() {
		Time = MaxTime;
		ShowUnitStats();
	}

	public void ShowPath(int maxAllowedLength) {
		GCTX ctx = GCTX.Instance;
		List<PPoint>.Enumerator __ptr = Path.GetEnumerator();
		while (__ptr.MoveNext()) {
			HexTile __t = ctx.Field.Get(__ptr.Current.X, __ptr.Current.Y);
			__t.GetComponent<SpriteRenderer>().color = maxAllowedLength > 0 ? new Color (1,1,1,0.2f) : new Color (1, 0, 0, 0.2f);
			maxAllowedLength--;
		};
	}

	public void HidePath() {
		GCTX ctx = GCTX.Instance;
		List<PPoint>.Enumerator __ptr = Path.GetEnumerator();
		while (__ptr.MoveNext()) {
			HexTile __t = ctx.Field.Get(__ptr.Current.X, __ptr.Current.Y);
			__t.GetComponent<SpriteRenderer>().color = Color.white;
		};
	}
}
