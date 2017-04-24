using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PaintSlinger : MonoBehaviour {

    public GameObject paintballPrefab;
    public Color paintballColor = Color.red;
    public float maxBackwardAngle = 75.0f;
    public float forwardAngleRatio = 1.0f;
    public int maxBalls = 20;
    public float minSize = 0.05f;
    public float maxSize = 0.25f;
    public float minSpeed = 10.0f;
    public float maxSpeed = 15.0f;
    public Vector3 handOffset = new Vector3(0.0f, 1.4f, 0.3f);
    public float maxPotatoRotation = 10f;

    public float brushDeadzone = 0.1f;
    public Vector3 brushHandIdlePos = new Vector3(0.4f, 0.25f, 0.6f);
    public Vector3 brushHandReadyPos = new Vector3(0.0f, 0.6f, 1.0f);
    public Vector3 brushTip = new Vector3(0.0f, 0.25f, 0.0f);

    public float brushAngularSpeed = 540.0f;
    public float dripInterval = 0.010f;

    private ToggleMouseLook toggleMouseLook;

    private Transform brushHand;
    private Transform brushHead;
    private float? aimingAngle = null;
    private float? pullbackAngle = null;
    private float? stoppingAngle = null;
    private bool launching = false;
    private float lastDrip = 0.0f;

	// Use this for initialization
	void Start () {
        toggleMouseLook = this.GetComponent<ToggleMouseLook>();
        brushHand = this.transform.Find("BrushHand");
        brushHead = this.transform.Find("BrushHand/Brush/Brush_Head");
	}

	// Update is called once per frame
	void Update () {
        if (launching)
        {
            Transform brushHand = this.transform.Find("BrushHand");

            pullbackAngle += Time.deltaTime * brushAngularSpeed;

            brushHand.localPosition = brushHandReadyPos;
            brushHand.localRotation = Quaternion.Euler(0, 0, aimingAngle.Value - 90) *
                                      Quaternion.Euler(pullbackAngle.Value, 0, 0);

            if (Time.time - lastDrip > dripInterval)
            {

                Vector3 ballWorldPos = brushHead.TransformPoint(Vector3.zero);
                float ballSize = (minSize + maxSize) / 2;
                float ballSpeed = (minSpeed + maxSpeed) / 2;
                Vector3 ballWorldVelocity = ballSpeed * brushHead.TransformDirection(Vector3.forward);

                GameObject ball = Instantiate<GameObject>(paintballPrefab, ballWorldPos, Quaternion.identity);
                ball.GetComponent<Renderer>().material.color = paintballColor;
                ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                ball.GetComponent<Rigidbody>().velocity = ballWorldVelocity;
                ball.GetComponent<Rigidbody>().AddTorque(maxPotatoRotation * Random.insideUnitSphere);
            }

            if (pullbackAngle >= stoppingAngle)
            {
                launching = false;
                brushHand.localPosition = brushHandIdlePos;
                brushHand.localRotation = Quaternion.identity;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                toggleMouseLook.DisableMouseLook();
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = new Vector2(2 * (Input.mousePosition.x / Screen.width) - 1,
                                                    2 * (Input.mousePosition.y / Screen.height) - 1);
                float d = mousePosition.magnitude / Mathf.Sqrt(2);

                if (d > brushDeadzone)
                {
                    Transform brushHand = this.transform.Find("BrushHand");

                    aimingAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
                    pullbackAngle = -d * maxBackwardAngle;
                    stoppingAngle = -forwardAngleRatio * pullbackAngle;

                    brushHand.localPosition = brushHandReadyPos;
                    brushHand.localRotation = Quaternion.Euler(0, 0, aimingAngle.Value - 90) *
                                              Quaternion.Euler(pullbackAngle.Value, 0, 0);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Transform brushHand = this.transform.Find("BrushHand");

                if (aimingAngle.HasValue && pullbackAngle.HasValue)
                {
                    launching = true;
                }
                else
                {
                    brushHand.localPosition = brushHandIdlePos;
                    brushHand.localRotation = Quaternion.identity;
                }

                toggleMouseLook.EnableMouseLook();
            }
        }
	}
}
