using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PaintSlinger : MonoBehaviour
{

    public GameObject paintballPrefab;
    public Color paintballColor = Color.red;
    public float maxBackwardAngle = 75.0f;
    public float forwardAngleRatio = 1.0f;
    public int maxBalls = 20;
    public float minSize = 0.15f;
    public float maxSize = 0.4f;
    public float minSpeed = 15.0f;
    public float maxSpeed = 25.0f;
    public float maxPotatoRotation = 10f;

    public float brushDeadzone = 0.1f;
    public Vector3 brushHandIdlePos = new Vector3(0.4f, 0.25f, 0.6f);
    public Vector3 brushHandReadyPos = new Vector3(0.0f, 0.6f, 1.0f);

    public float brushAngularSpeed = 540.0f;

    private ToggleMouseLook toggleMouseLook;

    private Transform brushHand;
    private float? aimingAngle = null;
    private float? pullbackAngle = null;
    private float? stoppingAngle = null;
    private float currentAngle = 0.0f;
    private bool launching = false;

    // Use this for initialization
    void Start()
    {
        toggleMouseLook = this.GetComponent<ToggleMouseLook>();
        brushHand = this.transform.Find("BrushHand");
    }

    // Update is called once per frame
    void Update()
    {
        if (launching)
        {
            currentAngle += Time.deltaTime * brushAngularSpeed;

            brushHand.localPosition = brushHandReadyPos;
            brushHand.localRotation = Quaternion.Euler(0, 0, aimingAngle.Value - 90) *
                                      Quaternion.Euler(currentAngle, 0, 0);

            if (currentAngle >= stoppingAngle)
            {
                launching = false;
                aimingAngle = null;
                pullbackAngle = null;
                stoppingAngle = null;
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
                if (aimingAngle.HasValue && pullbackAngle.HasValue && stoppingAngle.HasValue)
                {
                    launching = true;
                    currentAngle = pullbackAngle.Value;

                    float angle = aimingAngle.Value * Mathf.Deg2Rad;
                    float d = pullbackAngle.Value / -maxBackwardAngle;

                    float alpha = d * maxBackwardAngle * Mathf.Deg2Rad;
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

                        Vector3 ballWorldPos = this.transform.TransformPoint(ballPos + brushHandReadyPos);
                        Vector3 ballWorldVelocity = this.transform.TransformVector(ballVelocity);

                        GameObject ball = Instantiate<GameObject>(paintballPrefab, ballWorldPos, Quaternion.identity);
                        ball.GetComponent<Renderer>().material.color = paintballColor;
                        ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                        ball.GetComponent<Rigidbody>().velocity = ballWorldVelocity;
                        ball.GetComponent<Rigidbody>().AddTorque(maxPotatoRotation * Random.insideUnitSphere);
                    }
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
