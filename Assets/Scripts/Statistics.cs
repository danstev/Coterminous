using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour {

    public int health = 0;
    public int maxHealth = 100;
    private bool dead = false;
    public Text healthText;
    
    //public Text hhh;

    void Update () {

        if(dead)
        {
            return;
        }

        if (health <= 0)
        {
            dead = true;
            Destroy();
        }

        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if(tag == "Player")
        {
            healthText.text = "Health : " + health;
        }
		
	}

    private void Destroy()
    {
        //Debug.Log("Death: " + gameObject.name );

        if(tag == "Player")
        {
            Debug.Log("Player: " + gameObject.name + " has died.");
        }
        else if(tag == "Enemy")
        {
            Debug.Log("Enemy: " + gameObject.name + " has died.");
            EnemyDeath();
        }
        else if(tag == "NPC")
        {
            Debug.Log("NPC: " + gameObject.name + " has died.");
        }
    }

    public void TakeDamage(int incoming)
    {
        health -= incoming;
        Debug.Log("Damage: " + incoming + " has been taken by: " + gameObject.name + ".");

        if(tag == "Enemy")
        {
            EnemyIntelligence ei = GetComponent<EnemyIntelligence>();
            if(ei.target == null)
            {
                //set target to the person attacking
            }
        }
    }

    private void EnemyDeath()
    {
        GetComponent<EnemyIntelligence>().SetDead();
        //CapsuleCollider col = GetComponent<CapsuleCollider>();
        //col.enabled = false;
        StartCoroutine(Fade(GetComponent<Renderer>()));
        Destroy(gameObject, 2f);
    }

    IEnumerator Fade(Renderer r)
    {
        float start = Time.time;
        while (Time.time <= start + 2f)
        {
            Color c = r.material.color;
            c.a = 1f - Mathf.Clamp01((Time.time - start) / 2f);
            r.material.color = c;
            yield return new WaitForEndOfFrame();
        }
    }

}
