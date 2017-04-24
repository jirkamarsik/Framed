using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Splashy : MonoBehaviour
{
    public Brush[] splashBrushes;
    public float splashMinScale = 4.0f;
    public float splashMaxScale = 12.0f;
    public float dropletMinAmount = 6.0f;
    public float dropletMaxAmount = 12.0f;
    public float dropletMinSize = 0.05f;
    public float dropletMaxSize = 0.2f;
    public float dropletMinReflection = 1.0f;
    public float dropletMaxReflection = 3.0f;
    public float dropletScatter = 3.0f;

    float BallSize
    {
        get
        {
            Vector3 ballDimensions = this.GetComponent<Collider>().bounds.size;
            return System.Math.Max(ballDimensions.x, System.Math.Max(ballDimensions.y, ballDimensions.z));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PaintTrigger"))
        {
            var surface = other.transform.parent.gameObject;
            var canvas = surface.GetComponent<InkCanvas>();
            var splashBrush = splashBrushes[Mathf.FloorToInt(Random.Range(0, splashBrushes.Length - 1e-20f))];

            float ballSize = BallSize;

            // Assuming that the object we hit is the standard Unity Plane
            float wallSizeX = 10 * surface.transform.localScale.x;
            float wallSizeZ = 10 * surface.transform.localScale.z;

            float scaleX = ballSize / wallSizeX;
            float scaleZ = ballSize / wallSizeZ;

            Random.InitState((int)(this.transform.position.x * (2 ^ 20)));
            float jitteredSplashScale = Random.Range(splashMinScale, splashMaxScale);
            splashBrush.ScaleU = jitteredSplashScale * scaleX;
            splashBrush.ScaleV = jitteredSplashScale * scaleZ;

            Vector3 ballWorldPos = this.transform.position;
            Vector3 ballSurfacePos = surface.transform.InverseTransformPoint(ballWorldPos);
            Vector2 ballUV = new Vector2((-ballSurfacePos.x + 5) / 10, (-ballSurfacePos.z + 5) / 10);

            if (ballUV.x + splashBrush.ScaleU / 2 < 0 ||
                ballUV.x - splashBrush.ScaleU / 2 > 1 ||
                ballUV.y + splashBrush.ScaleV / 2 < 0 ||
                ballUV.y - splashBrush.ScaleV / 2 > 1)
                return;

            splashBrush.Color = this.gameObject.GetComponent<Renderer>().material.color;

            canvas.PaintUVDirect(splashBrush, ballUV);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PaintSurface"))
        {
            int droplets = Mathf.FloorToInt(BallSize * Random.Range(dropletMinAmount, dropletMaxAmount));
            for (int i = 0; i < droplets; i++)
            {
                GameObject droplet = Instantiate(this.gameObject);
                droplet.transform.localScale = this.transform.localScale * Random.Range(dropletMinSize, dropletMaxSize);
                Vector3 dropletVelocity = this.GetComponent<Rigidbody>().velocity; // preserve
                dropletVelocity += Random.Range(dropletMinReflection, dropletMaxReflection) * collision.contacts[0].normal; // reflect
                dropletVelocity += dropletScatter * Random.insideUnitSphere; // jitter
                droplet.GetComponent<Rigidbody>().velocity = dropletVelocity;
            }

            Destroy(this.gameObject);
        }
    }
}
