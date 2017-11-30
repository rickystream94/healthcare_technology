using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

    public GameController gameController;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer>=3f)
        {
            gameController.AddHazard(true);
            gameController.AddHazard(false);
            gameController.IncreaseScore();
            timer = 0f;
        }
	}
}
