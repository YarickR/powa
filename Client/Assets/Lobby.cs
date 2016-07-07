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

	IEnumerator AsyncReqCoro(string URL, Action<JSONNode> retHandler) {
		WWW __www = new WWW( PConst.Server_URL + URL);
		while (!__www.isDone) {
			yield return __www;
		};
		if (!string.IsNullOrEmpty(__www.error)) {
			Debug.Log("Error requesting " +  PConst.Server_URL + URL + ", error:" + __www.error);
		} else {
			var __r = JSON.Parse(__www.text);
			if (__r["error"] != null) {
				Debug.Log("Called " + URL + ", got error " + __r["error"]["code"].AsInt + ":" + __r["error"]["message"].Value);
			} else {
				retHandler(__r);
			}
		};
	}


	public void GetPlayerId() {
		GCTX ctx = GCTX.Instance;
		string __u =  "/powa.get_player_id?name=" + WWW.EscapeURL(ctx.User.Name);
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			ctx.User.GlobalId = a["result"]["id"].AsInt;
			RefreshMatchList();
		}));

	}

	public void RefreshMatchList() {
		GCTX ctx = GCTX.Instance;
		GameObject __glom =  GameObject.Find("Canvas/Lobby/GettingListOfMatches");
		__glom.SetActive(true);
		GameObject __matchPre = Resources.Load<GameObject>("MatchPre");
		string __u = "/powa.get_match_list?id=" + ctx.User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			__glom.SetActive(false);
			GameObject __sv = GameObject.Find("Canvas/Lobby/MatchList/Viewport/Content");
			for (int __cc = __sv.transform.childCount - 1; __cc >= 0; --__cc) {
				GameObject.Destroy(__sv.transform.GetChild(__cc).gameObject);
            };
			__sv.transform.DetachChildren();

			int __y = 0;
			foreach (JSONNode __i in a["result"].AsArray) {
				int __matchId = __i[0].AsInt;
				GameObject __match = (GameObject)Instantiate(__matchPre, new Vector3(0, 0 - __y * 45, 0), Quaternion.identity);
				__match.transform.SetParent(__sv.transform, false);
				__match.GetComponent<MatchRow>().MatchId = __matchId;
				GameObject __name = __match.transform.Find("MatchName").gameObject;
				if (__name) {
					__name.GetComponent<Text>().text = __matchId.ToString();
				};
				__y++;
			};
		}));
	}

	public void CreateMatch() {
		GCTX ctx = GCTX.Instance;
		string __u = "/powa.create_match?id=" + ctx.User.GlobalId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			
			Debug.Log(a["result"]);
			RefreshMatchList();
		}));
	}

	public void JoinMatch(int matchId) {
		GCTX ctx = GCTX.Instance;
		if (matchId == 0) {
			return;
		};
		string __u = "/powa.join_match?id=" + ctx.User.GlobalId + "&match_id=" + matchId;
		StartCoroutine(AsyncReqCoro(__u, delegate(JSONNode a) {
			Debug.Log(a["result"]);
		}));
	}
	// Update is called once per frame
	void Update () {
	
	}
}