using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Splashy : MonoBehaviour {

    public Brush splashBrush;
    public float splashScale = 7.0f;
    public float stampInterval = 0.05f;
    public float splashTime = 0.15f;
    public float splashDecay = 0.9f;

    private float lastStampTime = 0.0f;

    void OnCollisionEnter(Collision collision)
    {
        Stamp(collision.gameObject, collision.contacts[0].point);
        transform.localScale *= splashDecay;
        StartCoroutine(DieLater(splashTime));
    }

    void OnCollisionStay(Collision collision)
    {
        Stamp(collision.gameObject, collision.contacts[0].point);
        transform.localScale *= splashDecay;
    }

    void Stamp(GameObject surface, Vector3 location)
    {
        if (Time.time - lastStampTime < stampInterval)
            return;

        var canvas = surface.GetComponent<InkCanvas>();

        if (canvas != null)
        {
            Vector3 ballDimensions = this.GetComponent<Collider>().bounds.size;
            float ballSize = System.Math.Max(ballDimensions.x, System.Math.Max(ballDimensions.y, ballDimensions.z));

            // Assuming that the object we hit is the standard Unity Plane
            float wallSizeX = 10 * surface.transform.localScale.x;
            float wallSizeZ = 10 * surface.transform.localScale.z;

            float scaleX = ballSize / wallSizeX;
            float scaleZ = ballSize / wallSizeZ;

            splashBrush.ScaleU = splashScale * scaleX;
            splashBrush.ScaleV = splashScale * scaleZ;

            canvas.Paint(splashBrush, location);

            lastStampTime = Time.time;
        }
    }

    IEnumerator DieLater(float deadline)
    {
        yield return new WaitForSeconds(deadline);
        Destroy(this.gameObject);
    }
}
