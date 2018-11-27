using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        var device = InputManager.ActiveDevice;

        transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * device.LeftStickX, Space.World);
        transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * device.LeftStickY, Space.World);

        Debug.Log(" " + device.Name + " " + device.LeftStickX.Value);
    }
}
