using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ColorPicker : MonoBehaviour {

    public Texture2D colorPicker;

    private ToggleMouseLook toggleMouseLook;

	// Use this for initialization
	void Start () {
        toggleMouseLook = this.GetComponent<ToggleMouseLook>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            toggleMouseLook.DisableMouseLook();
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
            toggleMouseLook.EnableMouseLook();
	}

    void OnGUI()
    {
        if (Input.GetMouseButton(1))
        {
            float onscreenSize = Mathf.Min(0.5f * Screen.width, 0.8f * Screen.height);
            GUI.DrawTexture(new Rect((Screen.width - onscreenSize) / 2,
                                     (Screen.height - onscreenSize) / 2,
                                     onscreenSize,
                                     onscreenSize),
                            colorPicker);

            Vector2 mousePos = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 relativeMousePos = mousePos - screenCenter;

            if (relativeMousePos != Vector2.zero)
            {
                float angle = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x);
                angle -= Mathf.PI / 2;
                if (angle < 0.0)
                {
                    angle += 2 * Mathf.PI;
                }

                float hue = 1 - angle / (2 * Mathf.PI);

                Color chosenColor = Color.HSVToRGB(hue, 1, 1);
                this.GetComponent<PaintSlinger>().paintballColor = chosenColor;
                Drawing.DrawLine(screenCenter, new Vector2(mousePos.x, Screen.height - mousePos.y), chosenColor, 10);
            }
        }
    }
}