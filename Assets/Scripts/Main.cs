using UnityEngine;
using System;
using System.Collections;

public class Main : MonoBehaviour {
	public bool isMoving = false;
	public GUIText GUIplayerInView;
	public GUIText GUIjoint;
	public GUIText GUIjointPosition;

	// Use this for initialization
	void Start () {
		//QualitySettings.vSyncCount = 1;
		GameObject Hand_Right;
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
				}
			}
		}

		else {
			GUIplayerInView.text = "no player";
		}
	}
}