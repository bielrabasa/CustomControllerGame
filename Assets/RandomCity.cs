using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class RandomCity : MonoBehaviour
{
    public int size;
    public float buildingWidth;
    public float buildingWidthRandomiser;
    public float buildingSeparation;
    public float height;
    public float floorHeight;
    public float vibration;

    public Gradient colours;
    
    // Start is called before the first frame update
    void Start()
    {
        float totalSize = size * buildingSeparation;
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.localScale = new Vector3(totalSize, 1, totalSize) / 10f;
        floor.transform.localPosition = new Vector3((totalSize - buildingSeparation) / 2f, 0, (totalSize - buildingSeparation) / 2f);
        floor.transform.parent = transform;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                float h = Random.Range(0, height) + floorHeight;
                float w = buildingWidth + Random.Range(-buildingWidthRandomiser, buildingWidthRandomiser);
                float d = buildingWidth + Random.Range(-buildingWidthRandomiser, buildingWidthRandomiser);
                cube.transform.localScale = new Vector3(w, h, d);

                Vector3 pos = new Vector3(i * buildingSeparation, h / 2f, j * buildingSeparation);
                pos.x += Random.Range(-vibration, vibration);
                pos.z -= Random.Range(-vibration, vibration);
                cube.transform.position = pos;

                cube.transform.parent = transform;

                cube.GetComponent<Renderer>().material.color = colours.Evaluate((h - floorHeight) / height);
            }
        }
    }
}
