using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOnCollide : MonoBehaviour {

    public GenerateTerrain gt;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            print(gameObject.name);
            string str = gameObject.name;
            string[] coords = str.Split(' ');
            int xCoord = int.Parse(coords[0]);
            int yCoord = int.Parse(coords[1]);

            gt.GenerateInArea(xCoord, yCoord);
        }
    }
}
