using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public int frameBufferCount;
	public GUIText GUIplayerInView;
	public GUIText GUIjointPosition;
	public GUIText GUIacceleration;
	public GUIText GUIisMoving;

	List<Vector3> jointCache = new List<Vector3>();
	Vector3 acceleration;

	// Use this for initialization
	void Start () {
		//QualitySettings.vSyncCount = 1;
		for (int i = 0; i < frameBufferCount; i++)
			jointCache.Add (new Vector3(0, 0, 0));

		acceleration = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

		if (playerID > 0) {
			GUIplayerInView.text = "Player present";

			int jointsCount = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
			for (int i = 0; i < jointsCount; i++) {

				if(KinectManager.Instance.IsJointTracked(playerID, 11)) {
					int joint = 11;

					Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, joint);
					GUIjointPosition.text = posJoint.ToString();

					acceleration = (posJoint - jointCache[jointCache.Count - 1]);

					if (!V3Equal(acceleration,Vector3.zero)) {
						GUIisMoving.text = "moving";
					}
					else {
						GUIisMoving.text = "-";
					}

					GUIacceleration.text = acceleration.ToString();

					//prepare jointcache for next frame
					jointCache.Insert(0, posJoint);
					jointCache.RemoveAt(jointCache.Count - 1);
					
				}
			}
		}

		else {
			GUIplayerInView.text = "no player";
		}
	}

	public bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude(a - b) < 0.0001;
	}
}