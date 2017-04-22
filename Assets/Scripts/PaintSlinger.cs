using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PaintSlinger : MonoBehaviour {

    public GameObject paintballPrefab;
    public float maxBackwardAngle = 75 * Mathf.Deg2Rad;
    public float forwardAngleRatio = 1;
    public int maxBalls = 10;
    public float minSize = 0.02f;
    public float maxSize = 0.2f;
    public float minSpeed = 10;
    public float maxSpeed = 15;
    public float handLength = 0.6f;
    public float handHeight = 1.4f;

    private MouseLook mouseLook;
    private float prevXSensitivity;
    private float prevYSensitivity;
    private bool prevCursorLock;

	// Use this for initialization
	void Start () {
        mouseLook = this.GetComponent<FirstPersonController>().m_MouseLook;
	}

    private void DisableMouseLook()
    {
        prevCursorLock = mouseLook.lockCursor;
        prevXSensitivity = mouseLook.XSensitivity;
        prevYSensitivity = mouseLook.YSensitivity;
        mouseLook.SetCursorLock(false);
        mouseLook.XSensitivity = 0;
        mouseLook.YSensitivity = 0;
    }

	private void EnableMouseLook()
    {
        mouseLook.SetCursorLock(prevCursorLock);
        mouseLook.XSensitivity = prevXSensitivity;
        mouseLook.YSensitivity = prevYSensitivity;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            DisableMouseLook();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = new Vector2(2 * (Input.mousePosition.x / Screen.width) - 1,
                                                2 * (Input.mousePosition.y / Screen.height) - 1);
            Debug.Log(mousePosition);
            float d = mousePosition.magnitude / Mathf.Sqrt(2);


            float alpha = d * maxBackwardAngle;
            float beta = forwardAngleRatio * alpha;
            int balls = Mathf.CeilToInt(d * maxBalls);
            float phi_step = (alpha + beta) / balls;

            for (int i = 0; i < balls; i++)
            {
                float p = (float)i / (float)balls;
                float phi = i * phi_step;
                float angle = phi - alpha;

                Vector3 ballPos = new Vector3(Mathf.Cos(angle), handHeight, Mathf.Sin(angle));
                float ballSize = minSize + Mathf.Sin(Mathf.PI * p) * (maxSize - minSize);
                float ballSpeed = minSpeed + Mathf.Sin(Mathf.PI * p) * (maxSpeed - minSpeed);
                Vector3 ballVelocity = ballSpeed * new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));

                GameObject ball = Instantiate<GameObject>(paintballPrefab, this.transform.TransformPoint(ballPos), Quaternion.identity);
                ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                ball.GetComponent<Rigidbody>().velocity = this.transform.TransformVector(ballVelocity);

            }

            EnableMouseLook();
        }
	}
}
