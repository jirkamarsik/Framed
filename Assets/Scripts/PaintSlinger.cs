using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PaintSlinger : MonoBehaviour {

    public GameObject paintballPrefab;
    public Color paintballColor = Color.red;
    public float maxBackwardAngle = 75 * Mathf.Deg2Rad;
    public float forwardAngleRatio = 1;
    public int maxBalls = 20;
    public float minSize = 0.05f;
    public float maxSize = 0.25f;
    public float minSpeed = 10;
    public float maxSpeed = 15;
    public float handLength = 0.6f;
    public Vector3 handOffset = new Vector3(0, 1.4f, 0.3f);
    public float maxPotatoRotation = 10f;

    private ToggleMouseLook toggleMouseLook;
    private MouseLook mouseLook;
    private float prevXSensitivity;
    private float prevYSensitivity;

	// Use this for initialization
	void Start () {
        toggleMouseLook = this.GetComponent<ToggleMouseLook>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            toggleMouseLook.DisableMouseLook();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = new Vector2(2 * (Input.mousePosition.x / Screen.width) - 1,
                                                2 * (Input.mousePosition.y / Screen.height) - 1);
            float d = mousePosition.magnitude / Mathf.Sqrt(2);
            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x);

            float alpha = d * maxBackwardAngle;
            float beta = forwardAngleRatio * alpha;
            int balls = Mathf.CeilToInt(d * maxBalls);
            float phi_step = (alpha + beta) / balls;

            for (int i = 0; i < balls; i++)
            {
                float p = i / (float)balls;
                float phi = i * phi_step;
                float ballAngle = phi - alpha;

                Vector3 ballPos = new Vector3(Mathf.Cos(ballAngle), 0, Mathf.Sin(ballAngle));
                float ballSize = minSize + Mathf.Sin(Mathf.PI * p) * (maxSize - minSize);
                float ballSpeed = minSpeed + Mathf.Sin(Mathf.PI * p) * (maxSpeed - minSpeed);
                Vector3 ballVelocity = ballSpeed * new Vector3(-Mathf.Sin(ballAngle), 0, Mathf.Cos(ballAngle));

                ballPos = new Vector3(ballPos.x * Mathf.Cos(angle), ballPos.x * Mathf.Sin(angle), ballPos.z);
                ballVelocity = new Vector3(ballVelocity.x * Mathf.Cos(angle), ballVelocity.x * Mathf.Sin(angle), ballVelocity.z);

                Vector3 ballWorldPos = this.transform.TransformPoint(ballPos + handOffset);
                Vector3 ballWorldVelocity = this.transform.TransformVector(ballVelocity);

                GameObject ball = Instantiate<GameObject>(paintballPrefab, ballWorldPos, Quaternion.identity);
                ball.GetComponent<Renderer>().material.color = paintballColor;
                ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                ball.GetComponent<Rigidbody>().velocity = ballWorldVelocity;
                ball.GetComponent<Rigidbody>().AddTorque(maxPotatoRotation * Random.insideUnitSphere);
            }

            toggleMouseLook.EnableMouseLook();
        }
	}
}
