using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TreeEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PerlinCave : MonoBehaviour
{
    public int size;
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (PerlinNoise3D(i * scale, j * scale, k * scale) > .5f) continue;

                    GameObject Block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Block.transform.position = new Vector3(i,j,k);
                    Block.transform.parent = transform;
                    Block.name = "Block";
                }
            }
        }
    }
    public static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);

        return xy * xz * yz * yx * zx * zy;
    }

    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
    }

}
