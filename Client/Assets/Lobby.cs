using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;
using SimpleJSON;

[Serializable]
public class TNTReply {
	public int id;
	public object result;
}

public class Lobby : MonoBehaviour {

	// Use this for initialization
		
	void Start () {
		GCTX ctx = GCTX.Instance;
		ctx.User = new PPlayer(0, 0, Color.white); 
		ctx.User.Name = PPlayer.PlayerFirstNames[UnityEngine.Random.Range(0, PPlayer.PlayerFirstNames.GetLength(0) - 1)];
		ctx.User.Name += "  " + PPlayer.PlayerLastNames[UnityEngine.Random.Range(0, PPlayer.PlayerLastNames.GetLength(0) - 1)];
		GameObject __playerName =  GameObject.Find("Canvas/Lobby/PlayerName");
		__playerName.GetComponent<Text>().text = ctx.User.Name;
		GetPlayerId();
	}

	void GetPlayerId() {
		StartCoroutine(GetPlayerIdCoro());
	}

	IEnumerator GetPlayerIdCoro() {
		GCTX ctx = GCTX.Instance;
		string __u = PConst.Server_URL + "/powa.get_player_id?name=" + WWW.EscapeURL(ctx.User.Name);

		WWW __www = new WWW(__u);
		while (!__www.isDone) {
			yield return __www;
		};
		if (!string.IsNullOrEmpty(__www.error)) {
			Debug.Log("Error requesting " + __u + ", error:" + __www.error);
		} else {
			var __r = JSON.Parse(__www.text);
			ctx.User.GlobalId = __r["result"][0][0]["id"].AsInt;
		};
	}

	void RefreshMatchList() {
		StartCoroutine(RefreshMatchListCoro());
	}

	IEnumerator RefreshMatchListCoro() {
		GCTX ctx = GCTX.Instance;
		WWW __www = new WWW(WWW.EscapeURL(PConst.Server_URL + "/get_match_list"));
		while (!__www.isDone) {
			yield return __www;
		};
		if (!string.IsNullOrEmpty(__www.error)) {
			Debug.Log("Can't call " + PConst.Server_URL + "/get_match_list");
		} else {
			string __t = WWW.UnEscapeURL(__www.text);
			TNTReply __r = JsonUtility.FromJson<TNTReply>(__t);
			Debug.Log(__r.ToString());
		};
	}

	// Update is called once per frame
	void Update () {
	
	}
}
