﻿using System;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject fractured;

    private PlayerScore score;
    private Rigidbody rigidbody;
    public AudioSource shatterSound;
    public AudioSource coinCollect;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        score = GetComponent<PlayerScore>();

        AudioSource[] audios = GetComponents<AudioSource>();
        shatterSound = audios[0];
        coinCollect = audios[1];
    }

    void OnCollisionEnter(Collision col)
    {
        if (gameManager.active)
        {
            if (col.gameObject.tag == "Obstacle")
            {
                shatterSound.Play();
                GetComponent<UserControl>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                Vector3 pos = transform.position;
                Instantiate(fractured, pos, Quaternion.identity);
                FindObjectOfType<FollowPlayer>().enabled = false;
                gameManager.EndGame();
            }
            else if (col.gameObject.tag == "Target")
            {
                shatterSound.Play();
                GetComponent<UserControl>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                Vector3 pos = transform.position;
                Instantiate(fractured, pos, Quaternion.identity);
                FindObjectOfType<FollowPlayer>().enabled = false;
                switch (col.gameObject.name)
                {
                    case "Center":
                        score.AddScore(10);
                        break;
                    case "Middle":
                        score.AddScore(5);
                        break;
                    case "Outer":
                        score.AddScore(2);
                        break;
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (gameManager.active)
        {
            if (collider.tag == "Boundary")
            {
                GetComponent<UserControl>().enabled = false;
                FindObjectOfType<FollowPlayer>().enabled = false;
                gameManager.EndGame();
            }
            else if (collider.tag == "CoinItem")
            {
                coinCollect.Play();
                Destroy(collider.gameObject);
                score.AddScore(1);
            }
        }
    }
}
