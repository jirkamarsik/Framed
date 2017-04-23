using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepBlankTexture : MonoBehaviour {

    public int textureWidth = 2048;
    public int textureHeight = 2048;
    public Color color = Color.white;

	void Awake () {
        Texture2D blankTexture = new Texture2D(textureWidth, textureHeight);
        Color32[] initialColors = new Color32[textureWidth * textureHeight];

        for (int i = 0; i < initialColors.Length; i++)
            initialColors[i] = color;

        blankTexture.SetPixels32(initialColors);
        blankTexture.Apply();

        this.GetComponent<Renderer>().material.SetTexture("_MainTex", blankTexture);
	}
}
