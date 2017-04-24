using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ToggleMouseLook : MonoBehaviour {

    private MouseLook mouseLook;
    private float prevXSensitivity;
    private float prevYSensitivity;

	// Use this for initialization
	void Start () {
        mouseLook = this.GetComponent<FirstPersonController>().m_MouseLook;
	}

    public void DisableMouseLook()
    {
        prevXSensitivity = mouseLook.XSensitivity;
        prevYSensitivity = mouseLook.YSensitivity;
        mouseLook.SetCursorLock(false);
        Cursor.lockState = CursorLockMode.Confined;
        mouseLook.XSensitivity = 0;
        mouseLook.YSensitivity = 0;
    }

	public void EnableMouseLook()
    {
        mouseLook.SetCursorLock(true);
        mouseLook.XSensitivity = prevXSensitivity;
        mouseLook.YSensitivity = prevYSensitivity;
    }
}
