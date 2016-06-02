using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PPlayer
{
	public int Money 	{get; set;}
	public int PlayerId {get; set;}
	public int EOTLevel { get; set; }
	public float EOMTS { get; set; }
	public Color BaseColor { get; set; }
	public List<GameObject> Units;

	public PPlayer (int newMoney, int newId, Color newColor)
	{
		Money = newMoney; 
		PlayerId = newId;
		BaseColor = newColor;
		Units = new List<GameObject>();
		EOTLevel = PConst.EOT_None;
		EOMTS = 0;	
	}

}
