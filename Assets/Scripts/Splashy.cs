using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Splashy : MonoBehaviour {

    public Brush splashBrush;
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
        get {
            Vector3 ballDimensions = this.GetComponent<Collider>().bounds.size;
            return System.Math.Max(ballDimensions.x, System.Math.Max(ballDimensions.y, ballDimensions.z));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Stamp(collision.gameObject, collision.contacts[0].point);

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

    void Stamp(GameObject gameObject, Vector3 location)
    {
        var canvas = gameObject.GetComponent<InkCanvas>();

        if (canvas != null)
        {
            float ballSize = BallSize;

            // Assuming that the object we hit is the standard Unity Plane
            float wallSizeX = 10 * gameObject.transform.localScale.x;
            float wallSizeZ = 10 * gameObject.transform.localScale.z;

            float scaleX = ballSize / wallSizeX;
            float scaleZ = ballSize / wallSizeZ;

            float jitteredSplashScale = Random.Range(splashMinScale, splashMaxScale);
            splashBrush.ScaleU = jitteredSplashScale * scaleX;
            splashBrush.ScaleV = jitteredSplashScale * scaleZ;

            canvas.Paint(splashBrush, location);
        }
    }
}
