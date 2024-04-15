using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] private int health;
    [SerializeField] private int points;

    //[SerializeField] private Slider slider;

    [SerializeField] Color[] colors;

    private int indexColor = 0;

    private int healthMax = 10;
    public string color = "";
    private bool isCollision;

    private SpriteRenderer spr;

    public static Action<float> onUpdateHealth;
    public static Action<float> onTakeDamage;
    public static Action<int> onUpdatePoints;

    public static Action<bool> onGameOver;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        ChangeColor(0);
    }

    public void ChangeColor( int i)
    {
        if (isCollision) return;

        indexColor = Mathf.Clamp(indexColor + i, 0, 2);

        switch (indexColor)
        {
            case 0:
                color = "Red";
                break;
            case 1:
                color = "Blue";
                break;
            case 2:
                color = "Yellow";
                break;
        }

        spr.color = colors[indexColor];
    }

    public void ChangeColor(Color c, string color)
    {
        if (isCollision) return;

        spr.color = c;
        this.color = color;
    }

    public void UpdateHeath(int valor)
    {
        health = Mathf.Clamp(health + valor, 0, healthMax);
        //slider.value = (float)health / (float)healthMax;

        float value = (float)health / (float)healthMax;
        onUpdateHealth?.Invoke(value);

        if (health <= 0)
            onGameOver?.Invoke(false);
    }

    public void Updatestaygame()
    {
        onUpdatePoints?.Invoke(points);

        if (points == 30)
            onGameOver?.Invoke(true);
    }

    //public void AddHealth()
    //{
    //    health = Mathf.Clamp(health + 2, 0, healthMax);

    //    float value = (float)health / (float)healthMax;
    //    onUpdateHealth?.Invoke(value);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (collision.gameObject.TryGetComponent(out Obstacle obstacle))
           {
                isCollision = true;

                if (obstacle.Color != color)
                {
                    UpdateHeath(-1);
                    onTakeDamage?.Invoke(-3);
                }
                   

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isCollision = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Heart"))
        {
            UpdateHeath(2);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Coin"))
        {
            points += 10;
            onUpdatePoints?.Invoke(points);
            Destroy(collision.gameObject);

            Updatestaygame();
        }
    }
}
