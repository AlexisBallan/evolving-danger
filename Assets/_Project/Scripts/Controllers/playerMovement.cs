using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement du personnage

    private Rigidbody2D rb;
    private Vector2 movement;
    private SPUM_Prefabs spum_prefabs;
    private bool isRunning = false; // Indicateur de course
    private bool facingRight = false;



    void Start()
    {
        spum_prefabs = GetComponentInChildren<SPUM_Prefabs>();

        if (spum_prefabs == null)
        {
            Debug.LogWarning("ChildScript non trouvé sur l'objet enfant !");
        } 
        else
        {
            spum_prefabs.PopulateAnimationLists();
            spum_prefabs.OverrideControllerInit();
        }


        rb = GetComponent<Rigidbody2D>(); // Récupère le Rigidbody2D pour le mouvement physique
    }

    void Update()
    {
        // Récupère les entrées de l’utilisateur pour les axes X et Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        bool wasRunnig = isRunning;
        isRunning = movement.sqrMagnitude > 0.01f;

        if (isRunning != wasRunnig) 
        {
            if (isRunning)
            {
                spum_prefabs.PlayAnimation(PlayerState.MOVE, 0);
            } 
            else
            {
                spum_prefabs.PlayAnimation(PlayerState.IDLE, 0);
            }
        }

        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }

    }

    void FixedUpdate()
    {
        // Déplace le personnage en multipliant la direction par la vitesse de déplacement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        // Change l'orientation en inversant l'axe X de l'échelle
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
