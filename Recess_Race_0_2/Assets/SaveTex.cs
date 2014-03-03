using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class SaveTex : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D l_texture = null;
 
        
        
            l_texture = renderer.material.mainTexture as Texture2D;
        
 
		Color[] l_pixels = l_texture.GetPixels();
 
        Texture2D l_newTexture = new Texture2D(l_texture.width, l_texture.height, TextureFormat.ARGB32, false);
 
        l_newTexture.SetPixels(l_pixels);
 
        var texBytes = l_newTexture.EncodeToPNG();
 
        string fileName = "myFile.png";
 
        if (fileName.Length > 0) {
		FileStream f = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
		BinaryWriter b = new BinaryWriter(f);
		for (var i = 0; i < texBytes.Length; i++) b.Write(texBytes[i]);
		b.Close();  
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
