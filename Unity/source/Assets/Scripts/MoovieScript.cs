using UnityEngine;
using System.Collections;

public class MoovieScript : MonoBehaviour {

    // Use this for initialization
    void Start () {

		Handheld.PlayFullScreenMovie("Main.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        Application.LoadLevel("GameScene");


    }

    // Update is called once per frame
    void Update () {
	
	}
}
