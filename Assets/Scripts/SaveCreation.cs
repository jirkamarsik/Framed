using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCreation : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SaveTheThing();
        }
    }

    void SaveTheThing()
    {
        GameObject[] floorTiles = new GameObject[] { GameObject.Find("Splashable Tile f4"),
                                                     GameObject.Find("Splashable Tile f3"),
                                                     GameObject.Find("Splashable Tile f2"),
                                                     GameObject.Find("Splashable Tile f1") };

        var bigTex = new Texture2D(4096, 4096);

        Vector2[] offsets = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(2048, 0),
            new Vector2(0, 2048),
            new Vector2(2048, 2048)
        };

        for (int i = 0; i < 4; i++)
        {
            var tile = floorTiles[i];
            RenderTexture renderTexture = tile.GetComponent<Es.InkPainter.InkCanvas>().GetPaintMainTexture("Floor");
            RenderTexture.active = renderTexture;
            bigTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), (int)offsets[i].x, (int)offsets[i].y);
        }


        bigTex.Apply();

        System.IO.File.WriteAllBytes(string.Format("screen{0}.png", new System.Random().Next(1000000)), bigTex.EncodeToPNG());
    }
}
