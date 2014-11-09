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
	
	List<List<Vector3>> jointCache = new List<List<Vector3>>();
	Vector3 globalAcceleration;
	Vector3 localAcceleration;

	int[] jointList = {0,1,3,4,5,7,8,9,11,13,15,17,19};
	//int[] jointList = {11};

	// Use this for initialization
	void Start () {
		List<Vector3> tempList = new List<Vector3>();
		for (int u = 0; u < frameBufferCount; u++) {
			tempList.Add(new Vector3(0, 0, 0));
		}
		for (int i = 0; i < 20; i++) {
			jointCache.Add(tempList);
		}

		globalAcceleration.Set(0, 0, 0);
		localAcceleration.Set(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

		if (playerID > 0) {
			GUIplayerInView.text = "Player present";
			globalAcceleration.Set(0, 0, 0);

			int jointsCount = jointList.Length;
			foreach (int i in jointList) {
				if(KinectManager.Instance.IsJointTracked(playerID, i)) {
					int joint = i;

					//prepare jointcache for next frame
					Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, joint);
					jointCache[i].Insert(0, posJoint);
					jointCache[i].RemoveAt(jointCache[i].Count - 1);
				}
			}

			for (int i = 0; i < jointsCount; i++) {
				localAcceleration = (jointCache[i][0] - jointCache[i][jointCache[i].Count - 1]);
				if (!V3Equal(localAcceleration, Vector3.zero)) {
					localAcceleration = V3Invert(localAcceleration);
					globalAcceleration += localAcceleration;
				}
			}

			if (!V3Equal (globalAcceleration, Vector3.zero)) {
				GUIisMoving.text = "moving";
			}
			else {
				GUIisMoving.text = "-";
			}

			GUIacceleration.text = globalAcceleration.ToString();
		}
		else {
			GUIplayerInView.text = "no player";
		}
	}

	public bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude(a - b) < 0.0001;
	}

	public Vector3 V3Invert(Vector3 input) {
		if (input.x < 0) {
			input[0] = (input.x * -1);
		}
		if (input.y < 0) {
			input[1] = (input.y * -1);
		}
		if (input.z < 0) {
			input[2] = (input.z * -1);
		}
		return input;
	}
}