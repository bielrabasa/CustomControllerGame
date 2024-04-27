using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PerlinCave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (float px = 0; px < 100; px++)
        {

            //Our Y 
            for (float py = 0; py < 100; py++)
            {
                //1 valor juntez, 2
                float Perlin1 = Mathf.PerlinNoise(px / 80, 0);
                float Perlin2 = Mathf.PerlinNoise(py / 80, 0);
                Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
                Vector3 pos = new Vector3(py - 0, Perlin1 * 90 * Perlin2, px - 0);
                GameObject Block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Block.transform.position = pos;
                Block.transform.parent = transform;
                Block.name = "Block";
                Block.GetComponent<Renderer>().material.color = newColor;

            }
        }
    }
}
