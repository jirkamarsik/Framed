using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Splashy : MonoBehaviour {

    public Brush splashBrush;
    public float splashScale = 3.0f;

    void OnCollisionEnter(Collision collision)
    {
        var canvas = collision.gameObject.GetComponent<InkCanvas>();

        if (canvas != null)
        {
            Vector3 ballDimensions = this.GetComponent<Collider>().bounds.size;
            float ballSize = System.Math.Max(ballDimensions.x, System.Math.Max(ballDimensions.y, ballDimensions.z));

            // Assuming that the object we hit is the standard Unity Plane
            float wallSizeX = 10 * collision.transform.localScale.x;
            float wallSizeZ = 10 * collision.transform.localScale.z;

            float scaleX = ballSize / wallSizeX;
            float scaleZ = ballSize / wallSizeZ;

            splashBrush.ScaleU = splashScale * scaleX;
            splashBrush.ScaleV = splashScale * scaleZ;

            canvas.Paint(splashBrush, collision.contacts[0].point);
        }

        Destroy(this.gameObject);
    }
}
