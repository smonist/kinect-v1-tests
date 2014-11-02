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

	// Use this for initialization
	void Start () {
		//QualitySettings.vSyncCount = 1;
		/*for (int i = 0; i < 25; i++) {
			Debug.Log(i.ToString());
			jointCache.Add(new List<Vector3>());
				for (int u = 0; u < frameBufferCount; u++) {
				jointCache[0].Add(new Vector3 (0, 0, 0));
			}
		}*/

		//create nested List and fill it with zero'd Vector3
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

			int jointsCount = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
			//GUIjointPosition.text = jointsCount.ToString();
			for (int i = 0; i < jointsCount; i++) {

				if(KinectManager.Instance.IsJointTracked(playerID, i)) {
					int joint = i;

					//prepare jointcache for next frame
					Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, joint);
					jointCache[i].Insert(0, posJoint);
					jointCache[i].RemoveAt(jointCache[i].Count - 1);
				}
			}

			globalAcceleration.Set(0, 0, 0);
			for (int i = 0; i < jointsCount; i++) {
				localAcceleration = (jointCache[i][0] - jointCache[i][jointCache[i].Count - 1]);

				if (!V3Equal(localAcceleration, Vector3.zero))
					globalAcceleration += localAcceleration;
				}

			if (!V3Equal (globalAcceleration, Vector3.zero)) {
				GUIisMoving.text = "moving";
			}
			else {
				GUIisMoving.text = "-";
			}

			GUIacceleration.text = globalAcceleration.ToString();
			/*acceleration = (posJoint - jointCache[jointCache.Count - 1]);
			
			if (!V3Equal(acceleration,Vector3.zero)) {
				GUIisMoving.text = "moving";
			}
			else {
				GUIisMoving.text = "-";
			}
			
			GUIacceleration.text = acceleration.ToString();
			*/
		}
		else {
			GUIplayerInView.text = "no player";
		}
	}

	public bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude(a - b) < 0.0001;
	}
}