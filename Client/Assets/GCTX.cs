using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Networking;
using SimpleJSON;

public class GCTX : Singleton<GCTX> {


	protected GCTX () {} 

	public PField Field = null;
	public List<PPlayer> Players;
	public List<JSONClass> ServerCommands;
	public PPlayer User = null;
	public int UserId = 0;
	public int CurrentMovePlayerId = 0;
	public int FightState = 0;
	public Vehicle SelectedVehicle, MovingVehicle, ShootingVehicle;
	public GameObject Lobby, FightUI, Timer;
	public bool ActivityLock, AI;

	private IEnumerator _longPollCoro;
	private ulong _lastMsgId, _lastCmdId;
	private float _lastEOMDTS;
	private Vector3 _lobbyCamPos;
	private Quaternion _lobbyCamRtr;
	private AI _aiMgr;
	public static void Log(object o) {
		if (Application.isEditor) {
			string __t =  System.DateTime.Now.ToString() + "(+" + Time.realtimeSinceStartup + " s) " + o.ToString();
			UnityEngine.Debug.Log(__t);
		} else {
			/*
			string __t = System.Environment.StackTrace.Split('\n')[2];
			__t = __t.Substring(__t.LastIndexOf(" in "));
			System.Diagnostics.Debug.WriteLine(String.Format("{0} ({1}) {2}\n\t{3}",System.DateTime.Now.ToString(), Time.realtimeSinceStartup, o.ToString(), __t));
			*/
			int __uid = Instance.User != null ? Instance.User.GlobalId : 0;
			System.Diagnostics.Debug.WriteLine(String.Format("* {0} ({1}) #{2} * {3}",System.DateTime.Now.ToString(), Time.realtimeSinceStartup, __uid, o.ToString()));

		};
	}
	public void InitLobby() {
		_longPollCoro = null;
		_lastMsgId = 0;
		_lastCmdId = 0;
		Camera __c = GameObject.Find("Eye").GetComponent<Camera>();
		_lobbyCamPos = __c.transform.localPosition;
		_lobbyCamRtr = __c.transform.localRotation;

		ActivityLock = false;
		AI = false;
		GameObject __ui = GameObject.Find("/UI");
		Lobby = __ui.transform.Find("LobbyPanel").gameObject;
		FightUI = __ui.transform.Find("FightUIPanel").gameObject;
		Timer = FightUI.transform.Find("TimerPanel").gameObject;
		User = new PPlayer(0, 0, Color.white); 
		User.Name = PPlayer.PlayerFirstNames[UnityEngine.Random.Range(0, PPlayer.PlayerFirstNames.GetLength(0) - 1)];
		User.Name += " " + PPlayer.PlayerLastNames[UnityEngine.Random.Range(0, PPlayer.PlayerLastNames.GetLength(0) - 1)];
		GameObject __playerName =  GameObject.Find("/UI/UserInfoPanel/UserName");
		if (__playerName) {
			__playerName.GetComponent<Text>().text = User.Name;
		};

		FightState = FIGHT_STATE.LOBBY;
		GetPlayerId();
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

	public PPlayer FindPlayerByGid(int globalId) {
		foreach (PPlayer __ret in Players) { 
			if (__ret.GlobalId == globalId) return __ret ;
		};
		return null;
	}

	public Vehicle FindVehicleById(int vehicleId, int playerId) {
		Vehicle __ret = null;
		if (vehicleId == 0) {
			return null;
		};
		Players.ForEach(delegate(PPlayer p) {
			if ((playerId == 0) || (p.GlobalId == playerId)) {
				p.Units.ForEach(delegate(Vehicle v) {
					if (v.Id == vehicleId) {
						__ret = v;
					};
				});
			}
		});
		return __ret;
	}

	public bool HisMoveNow( int globalId ) {
		return Players[CurrentMovePlayerId].GlobalId == globalId;
	}

	public void SetupField(string fieldDescr, int fieldSize) {
		if (Field == null) {
			Field = new PField(fieldSize, fieldSize);
		};
		HexTile __tile = Resources.Load<HexTile>("HexTile");
		Sprite[] __hexTiles = Resources.LoadAll<Sprite>("Terrain");
		for (int __y = 0; __y < Field.Height; __y++) {
			for (int __x = 0; __x < Field.Width; __x++) {
				
				HexTile __t;
				Vector3 __v = new Vector3(Vehicle.rect2hexX(__x, __y), (float)(__y * 0.01), Vehicle.rect2hexY(__x, __y));
				__t = ( HexTile)Instantiate(__tile,  __v, Quaternion.Euler(90, 0, 0));
				string __ts = fieldDescr.Substring((__y * Field.Height + __x) * 2, 2);
				__t.X = __x;
				__t.Y = __y;
				__t.Type = Convert.ToInt32(__ts, 16);
				__t.Occupied = !__t.Passable();
				int __tidx = UnityEngine.Random.Range(4, 15);
				if (__tidx > 11) {
					__tidx += 16;
				};
				__t.GetComponent<SpriteRenderer>().sprite = __hexTiles[__tidx];
				Field.Set(__x, __y, __t);
			};
		};


		for (int __y = 0; __y < Field.Height; __y++) {
			for (int __x = 0; __x < Field.Width; __x++) {
				
				HexTile __t = Field.Get(__x, __y);
				if (__t.Type != PConst.TType_Ground) {
					//GCTX.Log("Loading " + "Trees/Prefabs/" + PField.Obstacles[__t.Type]);
					GameObject __obs = Resources.Load<GameObject>("Trees/Prefabs/" + PField.Obstacles[__t.Type]);
					if (__obs) {
						Mesh __om = __obs.GetComponent<MeshFilter>().sharedMesh;
        				Bounds __omb = __om.bounds;
						Vector3 __v = new Vector3(Vehicle.rect2hexX(__x, __y), 0 - __omb.min.y , Vehicle.rect2hexY(__x, __y));
						Instantiate(__obs, __v, Quaternion.Euler(0, UnityEngine.Random.Range(0f,350f), 0));
					} else {
						GCTX.Log("Loading " + "Trees/Prefabs/" + PField.Obstacles[__t.Type] + " failed");
					}
 				};
			};
		};
	}

	public void CleanupField() {
		if (Field != null) {
			for (int __y = 0; __y < Field.Height; __y++) {
				for (int __x = 0; __x < Field.Width; __x++) {
					Destroy(Field.Get(__x, __y).gameObject);
				};
			};
			Field.Cleanup();
			Field = null;
		}; // Let GC do the rest
		Camera __c = GameObject.Find("Eye").GetComponent<Camera>();
		__c.transform.localPosition = _lobbyCamPos;
		__c.transform.localRotation = _lobbyCamRtr;
	}


	public void SetupCamera(int playerPos) {
		Camera __c = GameObject.Find("Eye").GetComponent<Camera>();
		if (playerPos % 2 != 0) {
			__c.transform.position = new Vector3(15, 73, 38);
			__c.transform.eulerAngles = new Vector3(110, 00, 180);
		} else {
			__c.transform.position = new Vector3(15, 73, -17);
			__c.transform.eulerAngles = new Vector3(70, 0, 0);
		};
	}

	public void SetupPlayers(JSONArray players) {
		Players = new List<PPlayer>();
		for (int __i = 0; __i < players.Count; __i++) {
			JSONClass __plr = players[__i].AsObject;
			int __pos = __plr["pos"].AsInt;
			Color __c = __pos % 2 == 0 ? Color.blue : Color.red;
			PPlayer __p;
			if (__plr["id"].AsInt == User.GlobalId) {
				__p = User;
				__p.Money = __plr["money"].AsInt;
				__p.BaseColor = __c;
				SetupCamera(__pos);
			} else {
				__p = new PPlayer (__plr["money"].AsInt, __pos, __c );
				__p.GlobalId = __plr["id"].AsInt;
				__p.Name = WWW.UnEscapeURL(__plr["name"].Value);
			};
			Players.Add(__p);
			__p.PlayerId = __pos;
		};
		Players.ForEach(delegate (PPlayer __p) { __p.Setup(User.PlayerId); });
	}

	public void CleanupPlayers() {
		Players.ForEach(delegate(PPlayer __p) { __p.Cleanup(); });
		Players.Clear();
	}

	public void SetupVehicles(JSONArray vehicles) {
		string[] __prefabNames = new string[5] {"", "Vehicle_Light", "Vehicle_LightRanged", "Vehicle_Medium", "Vehicle_MediumRanged"};
		PropertyInfo[] __props = Type.GetType("Vehicle").GetProperties(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
		foreach (JSONNode __v in vehicles) {
			int __globalId = __v["PlayerId"].AsInt;
			int __vType = __v["Type"].AsInt;
			PPlayer __player = null;
			Players.ForEach(delegate (PPlayer p) { 
				if (p.GlobalId == __globalId ) { 
					__player = p ; 
				} 
			});
			if (__player == null) {
				GCTX.Log("Vehicle for unknown player, skipping");
				continue;
			};
			string __pfName = (__player.PlayerId % 2) == 0 ? __prefabNames[__vType] + "Blue" : __prefabNames[__vType] + "Red"; 
			GameObject __prefab = Resources.Load<GameObject>(__pfName);
			GameObject __vh = (GameObject)Instantiate(__prefab); 
			Vehicle __vhl = __vh.GetComponent<Vehicle>();
			foreach (PropertyInfo __prop in __props) {
				if (__v[__prop.Name] != null) {
					__prop.SetValue(__vhl, Convert.ChangeType(__v[__prop.Name].AsInt, __prop.PropertyType) , null);
				};
			};
			__player.Units.Add(__vhl);
			__vhl.PlayerId = __globalId;
			__vhl.MoveTo(__vhl.X, __vhl.Y);
		};	
	}

	public void EndTurn() {
		Players.ForEach(delegate(PPlayer p) { 
			p.Units.ForEach(delegate(Vehicle v) {
				v.Time = v.MaxTime;
				v.AttackMode = false;
				if ((SelectedVehicle != null) && (v == SelectedVehicle)) {
					v.ShowVehicleInfo();
				} else {
					v.ShowUnitStats();
				}
			});
		});
		ToggleAttack(false);
	}

	public void UnselectVehicle(Vehicle vh) {
		vh.Path.ForEach(delegate (PPoint pp) { Field.Get(pp.X, pp.Y).GetComponent<SpriteRenderer>().color = Color.white; });
		vh.Path.Clear();
		Field.Get(vh.X, vh.Y).GetComponent<SpriteRenderer>().color = Color.white;
		vh.AttackMode = false;
		ToggleAttack(false);
	}

	public void SelectVehicle(Vehicle vh) {
		Field.Get(vh.X, vh.Y).GetComponent<SpriteRenderer>().color = new Color (1,1,1,0.2f);
		vh.ShowVehicleInfo();
		vh.AttackMode = false;
		ToggleAttack(false);
		int __quadrant = vh.X < (Field.Width / 2) ? 0 : 1;
		__quadrant += vh.Y < (Field.Height / 2) ? 0 : 2;


	}

	public void ToggleAttack(bool on) {
		if (SelectedVehicle == null) {
			return;
		};
		Vehicle __vh = SelectedVehicle;
		GameObject __o = FightUI.transform.Find("AttackPanel").gameObject;
		Image __btnImg = __o.GetComponent<Image>();
		if (!on) {
			__vh.AttackMode = false;
			__btnImg.color = Color.white;
			for (int __y = 0; __y < Field.Height; __y++) {
				for (int __x = 0; __x < Field.Width; __x++) {
					Field.Get(__x, __y).Highlight(false, Color.white);
				};
			};
		} else {
			if ((__vh.Armor == 0) || (__vh.Time < __vh.ShotTU) || !HisMoveNow(__vh.PlayerId)) {
				return;
			};
			__vh.AttackMode = true;
			__btnImg.color = Color.red;
			for (int __y = 0; __y < Field.Height; __y++) {
				for (int __x = 0; __x < Field.Width; __x++) {
					int __dist = Mathf.RoundToInt(Mathf.Sqrt(Math.Abs(__y - __vh.Y) * Math.Abs(__y - __vh.Y) + Math.Abs(__x - __vh.X) * Math.Abs(__x - __vh.X)));
					if (__dist <= __vh.Distance) {
						Field.Get(__x, __y).Highlight(true, new Color(1,1,1,0.5f));
					};
				};
			};
		};
	}

	public void ToggleAI() {
		AI = !AI;
		GameObject.Find("/UI/UserInfoPanel/LevelSign").GetComponent<Image>().color = AI ? Color.yellow : Color.grey;
		if (AI) {
			GCTX.Log(User.Name + " is under AI control");
			_aiMgr = new AI();
		} else {
			_aiMgr = null;
			GCTX.Log(User.Name + " back to manual");
		}
	}
	public void PassTheMove(int PlayerId, bool next = true) {
		if (next) {
			PlayerId = (PlayerId + 1) % Players.Count;
		};
		Players[PlayerId].EOMTS = UnityEngine.Time.time + PConst.Move_Time;
		CurrentMovePlayerId = PlayerId;
		GameObject __img = Timer.transform.Find("TimerFG").gameObject;
		__img.GetComponent<Image>().color = Players[PlayerId].BaseColor;
	}

	IEnumerator AsyncReqCoro(string URL, Action<JSONNode> retHandler) {
		GCTX.Log("Going to request " + PConst.Server_URL + URL);
		WWW __www = new WWW( PConst.Server_URL + URL);
		while (!__www.isDone) {
			yield return __www;
		};
		if (!string.IsNullOrEmpty(__www.error)) {
			GCTX.Log("Transport error:" + __www.error + " calling " +  PConst.Server_URL + URL);
		} else {
			GCTX.Log("Got " + __www.text);
			var __r = JSON.Parse(__www.text);
			if (__r["error"] != null) {	
				GCTX.Log("Lua error " +  __r["error"]["code"].AsInt + ":" + __r["error"]["message"].Value + " calling " + URL );
			} else {
				retHandler(__r);
			}
		};
	}

	public void GetPlayerId() {
		string __u =  "/powa.get_player_id?name=" + WWW.EscapeURL(User.Name);
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			User.GlobalId = a["result"][0][0]["id"].AsInt;
			RefreshFightList();
			if (User.GlobalId != 0) {
				if (_longPollCoro != null) {
					StopCoroutine(_longPollCoro);
				};
				_longPollCoro = LongPoller();
				StartCoroutine(_longPollCoro);
			};
		}));
	}

	public void RefreshFightList() {
		GameObject __glom =  Lobby.transform.Find("GettingListOfFights").gameObject;
		__glom.SetActive(true);
		GameObject __fightPre = Resources.Load<GameObject>("FightInfo");
		GameObject __fightListUserInfo = Resources.Load<GameObject>("FightListUserInfo");
		string __u = "/powa.get_fight_list?id=" + User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			__glom.SetActive(false);
			GameObject __sv = Lobby.transform.Find("FightList/Viewport/Content").gameObject;
			for (int __cc = __sv.transform.childCount - 1; __cc >= 0; --__cc) {
				GameObject.Destroy(__sv.transform.GetChild(__cc).gameObject);
      		};
			__sv.transform.DetachChildren();
			int __y = 1;
			JSONNode __fights = a["result"][0][0]["fights"];
			foreach (JSONNode __i in __fights.AsArray) {
				int __fightId = __i["id"].AsInt;
				GameObject __fight = (GameObject)Instantiate(__fightPre, new Vector3(0, 21 - __y * 42, 0), Quaternion.identity);
				__fight.transform.SetParent(__sv.transform, false);
				__fight.GetComponent<FightInfo>().FightId = __fightId;
				GameObject __name = __fight.transform.Find("FightName").gameObject;
				if (__name) {
					__name.GetComponent<Text>().text = __fightId.ToString();
				};

				GameObject __fightUsers = __fight.transform.Find("FightUsers").gameObject;
				int __playerNo = 1;
				foreach (JSONNode __j in __i["players"].AsArray) {
					GameObject __flui = (GameObject)Instantiate(__fightListUserInfo, new Vector3(0, 0, 0), Quaternion.identity);
					__flui.transform.Find("Level").gameObject.GetComponent<Text>().text = "1";
					__flui.transform.Find("UserName").gameObject.GetComponent<Text>().text = WWW.UnEscapeURL(__j.Value);
					__flui.transform.SetParent(__fightUsers.transform, false);
					__flui.SetActive(true);
					__playerNo++;
				};
				__y++;
			};
		}));
	}

	public void CreateFight() {
		string __u = "/powa.create_fight?id=" + User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			int __fightId= a["result"]["id"].AsInt;
			RefreshFightList();
		}));
	}

	public void JoinFight(int fightId) {
		if (fightId == 0) {
			return;
		};
		string __u = "/powa.join_fight?player_id=" + User.GlobalId + "&fight_id=" + fightId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			int __fightId = a["result"][0][0]["id"].AsInt;
			if ( __fightId != 0) {
				RefreshFightList();
			}
		}));
	}

	public void GetFightData() {
		string __u = "/powa.get_fight_data?player_id=" + User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			SetupField(a["result"][0][0]["field"].Value, a["result"][0][0]["size"].AsInt);
			SetupPlayers(a["result"][0][0]["players"].AsArray);
			FightState = FIGHT_STATE.RUNNING;
			PassTheMove(a["result"][0][0]["active_player"].AsInt, false);
			GetFightVehicles();
		}));
	}
	public void GetFightVehicles() {
		string __u = "/powa.get_fight_vehicles?player_id=" + User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			SetupVehicles(a["result"][0].AsArray);
		}));
	}

	IEnumerator CmdCoro(string URL, Action<JSONNode> retHandler) {
		GCTX.Log("Sending command " + PConst.Server_URL + URL);
		ulong __lastAckCmdId;
		do {
			__lastAckCmdId = 0;
			WWW __www = new WWW( PConst.Server_URL + URL);
			while (!__www.isDone) {
				yield return __www;
			};
			if (!string.IsNullOrEmpty(__www.error)) {
				GCTX.Log("Transport error " + __www.error + " calling " +  PConst.Server_URL + URL);
			} else {
				GCTX.Log("Got " + __www.text);
				var __r = JSON.Parse(__www.text);
				if (__r["error"] != null) {	
					GCTX.Log("Lua error " +  __r["error"]["code"].AsInt + ":" + __r["error"]["message"].Value + " calling "+ URL );
					break;
				} else {
					__lastAckCmdId = (ulong)__r["result"][0][0].AsInt;
					break;
				};
			};
			yield return new WaitForSeconds(1);
		} while (__lastAckCmdId == 0);
		if (__lastAckCmdId < _lastCmdId ) {
			GCTX.Log("Last sent command has id " + _lastCmdId + ", last acknowledged id: " + __lastAckCmdId);
		};
	}

	public void FightCmd(string data) {
		string __u =  "/powa.fight_cmd?cmd_id=" + ++_lastCmdId + "&player_id=" +  User.GlobalId + "&data=" + data;
		StartCoroutine(CmdCoro(__u, delegate(JSONNode a) { return ; }));
	}

	public void Send_VEHICLE_MOVE(Vehicle vh) {
		string __pathStr = "";
		// By definition this is sent by the user
		User.EOMTS = UnityEngine.Time.time;
		vh.Path.ForEach(delegate(PPoint p) {__pathStr += System.String.Format("{0:X2}{1:X2}", p.X, p.Y);});
		JSONClass __cmd = new JSONClass();
		__cmd.Add("cmd", new JSONData((int)PCmds.VEHICLE_MOVE));
		__cmd.Add("player", new JSONData((int)vh.PlayerId));
		__cmd.Add("vehicle", new JSONData((int)vh.Id));
		__cmd.Add("path", new JSONData(__pathStr));
		__cmd.Add("pathlen", new JSONData((int)vh.Path.Count));
		FightCmd(__cmd.ToJSON(0));
	}

	public void Send_VEHICLE_ATTACK(Vehicle vh, Vehicle target, int tgtX, int tgtY, List<PDamage> dmgList) {
		JSONClass __cmd = new JSONClass();
		// By definition this is sent by the user
		User.EOMTS = UnityEngine.Time.time;
		__cmd.Add("cmd", new JSONData((int)PCmds.VEHICLE_ATTACK));
		__cmd.Add("player", new JSONData((int)vh.PlayerId));
		__cmd.Add("vehicle", new JSONData((int)vh.Id));
		__cmd.Add("tpid", new JSONData(target != null ? (int)target.PlayerId : (int)0));
		__cmd.Add("tvid", new JSONData(target != null ? (int)target.Id : (int)0));
		__cmd.Add("target", new JSONData(System.String.Format("{0:X2}{1:X2}", tgtX, tgtY)));
		JSONClass __dmgs = new JSONClass();
		dmgList.ForEach(delegate (PDamage d) { 
			JSONClass __dmgItem = new JSONClass();
			__dmgItem.Add("dvid", new JSONData((int)d.Vhcl.Id));
			__dmgItem.Add("dmg", new JSONData((int)d.Damage));
			__dmgs.Add("dmgi", __dmgItem);
		});
		__cmd.Add("dmgl", __dmgs);
		FightCmd(__cmd.ToJSON(0));
	}

	public void Send_END_TURN() {
		JSONClass __cmd = new JSONClass();
		__cmd.Add("cmd", new JSONData((int)PCmds.END_TURN));
		__cmd.Add("player", new JSONData((int)User.GlobalId));
		__cmd.Add("state", new JSONData((int)User.EOTLevel));
		FightCmd(__cmd.ToJSON(0));
	}

	IEnumerator LongPoller() {
		do {
			string __url = PConst.Server_URL + PConst.LPURL + "?player_id=" + User.GlobalId + "&last_msg_id=" + _lastMsgId;
			GCTX.Log("Going to request " + __url);
			WWW __www = new WWW( __url);
			while (!__www.isDone) {
				yield return __www;
			};
			if (!string.IsNullOrEmpty(__www.error)) {
				GCTX.Log("Error requesting " +  __url + ", error:" + __www.error);
				yield return new WaitForSeconds(1);
			} else {
				var __r = JSON.Parse(__www.text);
				if (__r["error"] != null) {	
					GCTX.Log("Called " + __url + ", got error " + __r["error"]["code"].AsInt + ":" + __r["error"]["message"].Value);
					yield return new WaitForSeconds(1);
				} else {
					foreach (JSONNode __cmd in __r["result"][0].AsArray) {
						JSONClass __c = __cmd.AsObject;
						__c.Add("state", new JSONData(PCmdState.READY));
						__c.Add("flags", new JSONData(PCmdFlags.DEFAULT));
						_lastMsgId = (ulong)__c["msg_id"].AsInt;
						ServerCommands.Add(__c);
					};
				};
			};	
		} while (true);
	}


	public void ProcessServerCmdQueue() {
		Queue<int> __rq = new Queue<int>(); // Remove queue
		int __p = 0;
		ServerCommands.ForEach(delegate (JSONClass c) {
			if ((c["state"].AsInt == PCmdState.READY) && 
				((__p == 0) || ((c["flags"].AsInt & PCmdFlags.PARALLEL) != 0))) {
				/* Either this is the first command or it was marked as allowed for the out of order processing */
				ProcessServerCmd(c.AsObject);
			};
			if (c["state"].AsInt == PCmdState.DONE) {
				GCTX.Log("Command has been processed");
				__rq.Enqueue(__p);
			};
			__p++;
		}) ;
		while (__rq.Count > 0) {
			ServerCommands.RemoveAt(__rq.Dequeue());	
		};
		__rq = null;
	}

	public void ProcessServerCmd(JSONClass c) {
		int __pId, __vhId;
		Vehicle __vh, __tvh;
		PPoint __pp;
		c["state"].AsInt = PCmdState.PROCESSING;
		switch (c["data"]["cmd"].AsInt) {
			case PCmds.START_FIGHT: 
				GCTX.Log("START_FIGHT");		
				FightState = FIGHT_STATE.READY;
				GetFightData();
				c["state"].AsInt = PCmdState.DONE;
				break;
			case PCmds.PASS_MOVE: 
				GCTX.Log("PASS_MOVE");
				PassTheMove(c["data"]["active_player"].AsInt, false);
				c["state"].AsInt = PCmdState.DONE;
				break;
			case PCmds.VEHICLE_MOVE:
				GCTX.Log("VEHICLE_MOVE");
				int __pathLen = c["data"]["pathlen"].AsInt;
				string __pathStr = c["data"]["path"].Value;
				__vhId = c["data"]["vehicle"].AsInt;
				__pId = c["data"]["player"].AsInt;
				__vh = FindVehicleById(__vhId, __pId);
				if (__vh != null) {
					__vh.MoveCommand = c;
					for (int __i = 0; __i < c["data"]["pathlen"].AsInt; __i++) {
						__pp = new PPoint();
						__pp.X = Convert.ToInt32(__pathStr.Substring(__i * 4, 2), 16);
						__pp.Y = Convert.ToInt32(__pathStr.Substring(__i * 4 + 2, 2), 16);
						__vh.Path.Add(__pp);
					};
					__vh.PathStep = 0;
					__vh.LastMoveTS = 0;
					if (MovingVehicle != null) {
						/* if something is already moving, let's place it where it belongs and start over */
						__pp = MovingVehicle.Path[MovingVehicle.Path.Count - 1];
						MovingVehicle.MoveTo(__pp.X, __pp.Y);
						MovingVehicle.HidePath();
						MovingVehicle.Path.Clear();
					};
					MovingVehicle = __vh;
					FindPlayerByGid(__pId).EOMTS = UnityEngine.Time.time;
				} else {
					GCTX.Log("Can't find vehicle " + __vhId + " belonging to player " + __pId);
					c["state"].AsInt = PCmdState.DONE;
				};
				break;
			case PCmds.VEHICLE_ATTACK:
				GCTX.Log("VEHICLE_ATTACK");
				__pId = c["data"]["player"].AsInt;
				__vhId = c["data"]["vehicle"].AsInt;
				int __tvId = c["data"]["tvid"].AsInt;
				int __tpId = c["data"]["tpid"].AsInt;
				int __tgtX = Convert.ToInt32(c["data"]["target"].Value.Substring(0, 2), 16);
				int __tgtY = Convert.ToInt32(c["data"]["target"].Value.Substring(2, 2), 16);
				__vh = FindVehicleById(__vhId, __pId);
				__tvh = FindVehicleById(__tvId, __tpId);
				if (!__vh.Shoot(__tgtX, __tgtY, __tvh, c)) {
					c["state"].AsInt = PCmdState.DONE;
				};
				FindPlayerByGid(__pId).EOMTS = UnityEngine.Time.time;
				break;
			case PCmds.END_TURN:
				GCTX.Log("END_TURN");
				__pId = c["data"]["player"].AsInt;
				Players.ForEach(delegate(PPlayer p) {
					if (p.GlobalId == __pId) {
						p.EOTLevel = c["data"]["state"].AsInt;
						if (p.EOTLevel != 0) {
							p.EOMTS = UnityEngine.Time.time;
						};
						p.setEndTurnIndicator(p.EOTLevel);
					};
				});
				c["state"].AsInt = PCmdState.DONE;
				break;
			case PCmds.NEXT_TURN:
				GCTX.Log("NEXT_TURN");
				int __apId = c["data"]["active_player"].AsInt;
				Players.ForEach(delegate(PPlayer p) { p.NextTurn(); } );
				PassTheMove(__apId, false);
				c["state"].AsInt = PCmdState.DONE;
				break;
			case PCmds.END_FIGHT:
				GCTX.Log("END_FIGHT");
				FightState = FIGHT_STATE.LOBBY;
				CleanupField();
				CleanupPlayers();
				c["state"].AsInt = PCmdState.DONE;
				break;
			default:
				c["state"].AsInt = PCmdState.DONE;
				break;
		};
	}

	public void Update() {
		if (ServerCommands.Count > 0) {
			ProcessServerCmdQueue();
		};
		switch (FightState) {
			case 0: 
				InitLobby(); return ;
			case FIGHT_STATE.LOBBY:
			case FIGHT_STATE.PREPARING:
				if (Lobby.activeSelf == false) Lobby.SetActive(true);
				if (FightUI.activeSelf == true) FightUI.SetActive(false);
				return; 
			case FIGHT_STATE.READY:
				if (Lobby.activeSelf == true) Lobby.SetActive(false);
				if (FightUI.activeSelf == false) FightUI.SetActive(true);

				return;
			case FIGHT_STATE.FINISHED:
				if (Lobby.activeSelf == false) Lobby.SetActive(true);
				if (FightUI.activeSelf == true) FightUI.SetActive(false);
				return;
		};
		// 	case FIGHT_STATE.RUNNING:
		if (MovingVehicle != null) {
			Vehicle __vh = MovingVehicle;
			if ((__vh.Path.Count < 1) || (__vh.PathStep > __vh.Path.Count - 1) || (__vh.Time <= 0)) {
				__vh.FinishMove();
				MovingVehicle = null;
				Field.Get(__vh.X, __vh.Y).GetComponent<SpriteRenderer>().color = Color.white;
				if (__vh.MoveCommand != null) {
					/* We've moved the vehicle due to server command */
					__vh.MoveCommand["state"].AsInt = PCmdState.DONE;
					__vh.MoveCommand = null;
				};
			} else {
				if ((__vh.PathStep == 0) && (__vh.PlayerId == User.GlobalId)) {
					/* VEHICLE_MOVE command */	
					Send_VEHICLE_MOVE(__vh);
				};
				if ( Time.time - __vh.LastMoveTS >= (__vh.PlayerId == User.GlobalId ? 0.2 : 0.1)) {
					__vh.MoveStep();
				};
			};
		} else 
		if (ShootingVehicle != null) {
			//Nothing to do yet
		} else {
			PPlayer __p = Players[CurrentMovePlayerId];
			if (__p.EOMTS >= UnityEngine.Time.time) {
				float __diff = __p.EOMTS - UnityEngine.Time.time;
				if (UnityEngine.Time.time - _lastEOMDTS > 0.1) {
					GameObject __EOMTimer =  Timer.transform.Find("TimerFG").gameObject;
					float __scale = __diff > PConst.Move_Time ? 1 : Math.Max(__diff / PConst.Move_Time, 0);
					__EOMTimer.GetComponent<Image>().fillAmount = __scale;
					_lastEOMDTS = UnityEngine.Time.time;
					ActivityLock = ((__diff > 0 ) & (__p.GlobalId == User.GlobalId));
					if ((ActivityLock == true) && (AI == true) && (_aiMgr != null)) {
						_aiMgr.Step();
					};
				};
			};
		};
	}
}
