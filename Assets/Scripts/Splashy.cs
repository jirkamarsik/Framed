using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Splashy : MonoBehaviour {

    public Brush splashBrush;
    public float splashScale = 0.2f;

    void OnCollisionEnter(Collision collision)
    {
        var canvas = collision.gameObject.GetComponent<InkCanvas>();

        if (canvas != null)
        {
            splashBrush.Scale = splashScale * this.GetComponent<Collider>().bounds.size.x;
            canvas.Paint(splashBrush, collision.contacts[0].point);
        }

        Destroy(this.gameObject);
    }
}
