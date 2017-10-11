using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public Vector3 spawnValues;
	public float hazardCount;
	public float spawnRate;
	public float spawnStart;
	public float waveWait;
	public DetectJoints detectJoints;

	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnWaves ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator SpawnWaves()
	{
		//Wait for calibration
		while (!detectJoints.IsCalibrated ()) {
			yield return new WaitForSeconds (1);
		}

		yield return new WaitForSeconds (spawnStart);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnRate);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
