using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashySurface : MonoBehaviour {

    public int textureSize = 2048;

    private Texture2D splashTexture;
    private Color32[] cpuTexture;
    private Color32[] buffer;

	// Use this for initialization
	void Start () {
        splashTexture = new Texture2D(textureSize, textureSize);
        cpuTexture = new Color32[splashTexture.width * splashTexture.height];
        for (int i = 0; i < cpuTexture.Length; i++)
            cpuTexture[i] = Color.white;
        buffer = new Color32[cpuTexture.Length];
        splashTexture.SetPixels32(cpuTexture);
        this.GetComponent<Renderer>().material.mainTexture = splashTexture;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PaintBall"))
        {
            RaycastHit hit;
            ContactPoint contact = collision.contacts[0];
            Physics.Linecast(contact.point, contact.point + contact.normal, out hit, ~LayerMask.NameToLayer("SplashySurfaces"));
            SmearDroplet(hit.textureCoord, collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

    public void SmearDroplet(Vector2 landingUV, GameObject paintBall)
    {
        int x_c = Mathf.FloorToInt(landingUV.x * splashTexture.width);
        int y_c = Mathf.FloorToInt(landingUV.y * splashTexture.height);

        Color32 color = paintBall.GetComponent<Renderer>().material.color;
        Vector3 paintballSizeVec = paintBall.GetComponent<Collider>().bounds.size;
        float paintballSize = System.Math.Max(paintballSizeVec.x, System.Math.Max(paintballSizeVec.y, paintballSizeVec.z));
        Vector3 surfaceSizeVec = this.GetComponent<Collider>().bounds.size;
        float surfaceSize = System.Math.Max(surfaceSizeVec.x, System.Math.Max(surfaceSizeVec.y, surfaceSizeVec.z));
        int r = (int)((paintballSize / surfaceSize) * splashTexture.width);

        int x_block = System.Math.Max(0, x_c - r);
        int y_block = System.Math.Max(0, y_c - r);
        int blockWidth = System.Math.Min(splashTexture.width, x_c + r + 1) - x_block;
        int blockHeight = System.Math.Min(splashTexture.height, y_c + r + 1) - y_block;

        int i = 0;
        for (int y = y_block; y < y_block + blockWidth; y++)
            for (int x = x_block; x < x_block + blockHeight; x++)
            {
                if ((x - x_c) * (x - x_c) + (y - y_c) * (y - y_c) <= r * r)
                    cpuTexture[y * splashTexture.width + x] = color;
                buffer[i] = cpuTexture[y * splashTexture.width + x];
                i++;
            }

        splashTexture.SetPixels32(x_block, y_block, blockWidth, blockHeight, buffer);
        splashTexture.Apply();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
