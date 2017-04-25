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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }

    public void DisableMouseLook()
    {
        prevXSensitivity = mouseLook.XSensitivity;
        prevYSensitivity = mouseLook.YSensitivity;
        mouseLook.SetCursorLock(false);
        mouseLook.UpdateCursorLock();
        Cursor.lockState = CursorLockMode.Confined;
        mouseLook.XSensitivity = 0;
        mouseLook.YSensitivity = 0;
    }

	public void EnableMouseLook()
    {
        mouseLook.SetCursorLock(true);
        mouseLook.UpdateCursorLock();
        mouseLook.XSensitivity = prevXSensitivity;
        mouseLook.YSensitivity = prevYSensitivity;
    }
}
