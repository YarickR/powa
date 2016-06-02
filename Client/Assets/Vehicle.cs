using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour {
	public GameObject Projectile, UnitStats;
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
	private int _type;
	public int  Type { get { return _type ; } set { _type = value; } }
	public int PlayerId { get; set; }
	public int Id { get; set; }
	public List<PPoint> Path;
	public int PathStep { get; set; }
	public float LastMoveTS { get; set; }
	public bool AttackMode { get; set; }
	private Material[] _allMaterials;
	private Color[] _matEmiColors;
	private GameObject _usp; // UnitStats panel
	private UnitStats _usps; // UnitStats panel script
	private Camera _eye;
	private float _flashStopTS;
	private Collider _collider;
	private RectTransform _uspt; // UnitStats panel transform

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
		Vector3 __usWC = new Vector3(transform.position.x, transform.position.y, transform.position.z); //World coordinates
		Vector3 __usSC = _eye.WorldToScreenPoint(__usWC); // Screen coordinates
		if (PlayerId == ctx.UserId) {
			_usp = (GameObject)Instantiate(UnitStats, __usSC, Quaternion.identity);
			GameObject __cvs = GameObject.Find("Canvas") ;
			_usp.transform.SetParent(__cvs.transform, false);
			_uspt = _usp.GetComponent<RectTransform>();
			_usps = _usp.GetComponent<UnitStats>();
		} else {
			_usp = null;
		};
		_collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_flashStopTS != 0) {
			if ( _flashStopTS < UnityEngine.Time.time ) {
				for (int __i = 0; __i < _allMaterials.GetLength(0); __i++) {
					_allMaterials[__i].SetColor("_EmissionColor", _matEmiColors[__i]);
				};
				_flashStopTS = 0;
			};
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
		ctx.FieldCells[Y, X].Occupied = false;
		transform.position = new Vector3(rect2hexX(newX, newY), (float)0.01, rect2hexY(newX, newY));
		transform.rotation = Quaternion.identity;
		ctx.FieldCells[newY, newX].Occupied = true;
		X = newX;
		Y = newY;
	}
	void OnMouseDown() {
		GCTX ctx = GCTX.Instance;
		if (ctx.SelectedVehicle != null) {
			Vehicle __vh = ctx.SelectedVehicle.GetComponent<Vehicle>();
			if ((__vh.AttackMode) && (ctx.SelectedVehicle != this.gameObject) && (__vh.PlayerId != PlayerId)) {
				__vh.Shoot(X, Y, this);
				return;
			};
			ctx.UnselectVehicle(ctx.SelectedVehicle);
		};
		ctx.SelectedVehicle = this.gameObject;
		ctx.SelectVehicle(this.gameObject);
	}
	void OnMouseUp() {
		
	}

	public int FindPathPoint(int pX, int pY) {
		if (Path.Count == 0) {
			return -1;
		};
		if ((pX == X) && (pY == Y)) {
			return 0;
		};
		for (int __i = 0; __i < Path.Count; __i++) {
			if ((Path[__i].X == pX) && (Path[__i].Y == pY)) {
				return __i;
			};
		};
		return -1;
	}
	public void RemovePathPoints(int startIdx) {
		GCTX ctx = GCTX.Instance;
		while (Path.Count > startIdx) {
			int __x = Path[Path.Count - 1].X;
			int __y = Path[Path.Count - 1].Y;
			ctx.FieldCells[__y, __x].GetComponent<SpriteRenderer>().color = Color.white;
			Path.RemoveAt(Path.Count - 1);
		};
	}

	public void Shoot(int tgtX, int tgtY, Vehicle vh) {
		GCTX ctx = GCTX.Instance;
		if ((Armor <= 0) || (Time < ShotTU) || (ctx.CurrentMovePlayerId != PlayerId)) {
			return;
		};
		if ((Type == PConst.VType_LightRanged) || (Type == PConst.VType_Medium)) {
			if (vh == null) {
				return;
			};
			tgtX = vh.GetComponent<Vehicle>().X;
			tgtY = vh.GetComponent<Vehicle>().Y;
		};
		if (!ctx.FieldCells[tgtY, tgtX].Highlighted) {
			return;
		};
		Vector3 __v = new Vector3(rect2hexX(X, Y), 1, rect2hexY(X, Y));
		Vector3 __t = new Vector3(rect2hexX(tgtX, tgtY), 1, rect2hexY(tgtX, tgtY));
		GameObject __proj = (GameObject)Instantiate(Projectile, __v, Quaternion.identity);
		Bullet __b = __proj.GetComponent<Bullet>();
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

		if (ctx.SelectedVehicle == this.gameObject) {
			ShowVehicleInfo();
		};
		AttackMode = false;
		ctx.ToggleAttack(false);
		ctx.ShootingVehicle = this.gameObject;
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
		if (this.gameObject == ctx.SelectedVehicle) {
			ShowVehicleInfo();
		} else {
			ShowUnitStats();
		};
	}

	void OnGUI() {
		GCTX ctx = GCTX.Instance;
		if (PlayerId == ctx.UserId) {
			Vector3 __s = _uspt.transform.localScale;
			__s.x  = 14 / _eye.fieldOfView;
			_uspt.transform.localScale = __s;
			Bounds __b = _collider.bounds;
			Vector3 __min = _eye.WorldToScreenPoint(__b.min);
			Vector3 __max = _eye.WorldToScreenPoint(__b.max);
			Vector3 __ctr = _eye.WorldToScreenPoint(__b.center);
			Vector3 __usSC = new Vector3(__ctr.x , __max.y, __min.z); 

			__usSC.x -= (_uspt.rect.width * __s.x) / 2;
			_usp.transform.position = __usSC;
		};
	}

	public void ShowVehicleInfo() {
		GameObject __unitImg =  GameObject.Find("Canvas/UnitData/_unitImg");
		Image __re = __unitImg.GetComponent<Image>();
		__re.sprite = GetComponent<SpriteRenderer>().sprite;
		GameObject __timeUnits =  GameObject.Find("Canvas/UnitData/_timeUnits");
		__timeUnits.GetComponent<Text>().text = Time.ToString();
		GameObject __armUnits =  GameObject.Find("Canvas/UnitData/_armUnits");
		__armUnits.GetComponent<Text>().text = Armor.ToString();
		ShowUnitStats();
	}

	public void ShowUnitStats() {
		if (_usp != null) {
			_usps.TUS.value = Armor > 0 ? Mathf.Round((Time * 100)/MaxTime) : 0;
			_usps.Armor.value = Armor > 0 ? Mathf.Round((Armor * 100)/MaxArmor) : 0;
		}
	}
	
}
