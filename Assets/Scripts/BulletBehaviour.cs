using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public int damage;

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(gameObject.name + " hit: " + other.collider.name );
        other.transform.SendMessage(("TakeDamage"), damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }

}

