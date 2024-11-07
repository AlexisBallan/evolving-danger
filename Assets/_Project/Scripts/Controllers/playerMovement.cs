using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Vitesse de déplacement du personnage
    public float dashSpeed = 30;
    public int cooldownBasicAttack = 2;
    public int cooldownDistanceAttack = 6;
    public int cooldownDash = 10;
    public bool canAttack, canDash, canTrhow = false;
    public GameObject throwKiwi; // Le prefab du caillou à lancer
    public float throwSpeed = 20f;
    public float dashDuration = 0.3f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 dashDirection;
    private SPUM_Prefabs spum_prefabs;
    private bool isRunning = false; // Indicateur de course
    private bool facingRight = false;
    private bool isAttacking, isThrowing, isDashing = false;
    private float lastBasicAttack, lastDistanceAttack, lastDash = 0;
    private float dashTime;




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
        if(!isAttacking && !isThrowing && !isDashing)
        {
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
       

        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame && lastBasicAttack < Time.time)
        {
            lastBasicAttack = Time.time + cooldownBasicAttack;
            Debug.Log("on attaque");
            isAttacking = true;
            isRunning = false;
            spum_prefabs.PlayAnimation(PlayerState.ATTACK, 0);
            StartCoroutine(IdleCoroutine());
        } else if (mouse.rightButton.wasPressedThisFrame && lastDistanceAttack < Time.time)
        {
            lastDistanceAttack = Time.time + cooldownDistanceAttack;
            Debug.Log("on lance ! ");
            isThrowing = true;
            isRunning = false;
            spum_prefabs.PlayAnimation(PlayerState.ATTACK, 2);
            StartCoroutine(IdleCoroutine());
            StartCoroutine(ThrowKiwi());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && lastDash < Time.time)
        {
            lastDash = Time.time + cooldownDash;
            Debug.Log("On dash");
            StartDash();

        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }


    }

    void FixedUpdate()
    {
        // Déplace le personnage en multipliant la direction par la vitesse de déplacement
        if(!isAttacking && !isThrowing && !isDashing)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator IdleCoroutine()
    {
        Debug.Log("début coroutine");
        
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        isThrowing = false;
    }

    IEnumerator ThrowKiwi()
    {
        yield return new WaitForSeconds(0.1f);
        // Créé une instance du caillou à la position du joueur
        GameObject kiwi = Instantiate(throwKiwi, transform.position, Quaternion.identity);

        // Calcule la direction vers la souris
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ignore l'axe Z pour un jeu en 2D
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Ajoute un mouvement au caillou dans la direction de la souris
        Rigidbody2D rbKiwi = kiwi.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rbKiwi.linearVelocity = direction * throwSpeed;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;

        // Calcule la direction du dash vers la souris
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ignore l'axe Z pour un jeu en 2D
        dashDirection = (mousePosition - transform.position).normalized;

        // Applique la vitesse du dash
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero; // Arrête le mouvement après le dash
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
