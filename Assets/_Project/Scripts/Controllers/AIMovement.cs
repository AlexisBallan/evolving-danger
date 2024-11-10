using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform[] allEntities; // Référence vers tous les autres personnages, y compris le joueur
    public float moveSpeed = 3f;
    public float detectionRadius = 15f;
    public float attackRange = 1.5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    // Pour l’exemple, on simule les sorties du réseau de neurones
    private float[] neuralOutputs;  // 0-7 : déplacements, 8 : action (0 rien, 1 attaque, 2 dash)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Simulation des sorties du réseau de neurones pour le test
        neuralOutputs = new float[9];
    }

    void Update()
    {
        // Met à jour les sorties du réseau de neurones en fonction de l'environnement actuel
        neuralOutputs = GetNeuralNetworkOutputs();

        // Interprète les sorties du réseau pour le déplacement et les actions
        ProcessMovement(neuralOutputs);
        ProcessActions(neuralOutputs);
    }

    float[] GetNeuralNetworkOutputs()
    {
        // Ici, implémentez le calcul du réseau de neurones ou connectez-le pour obtenir les sorties.
        // Par exemple, une sortie simulée pour les tests.
        return new float[9]; // Remplir avec des valeurs de sortie du réseau
    }

    void ProcessMovement(float[] outputs)
    {
        // Mapping des sorties pour la direction de déplacement (0-7 pour 8 directions possibles)
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized, new Vector2(1, -1).normalized
        };

        int moveIndex = Mathf.RoundToInt(outputs[0]);
        if (moveIndex >= 0 && moveIndex < directions.Length)
        {
            moveDirection = directions[moveIndex];
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    void ProcessActions(float[] outputs)
    {
        // Sortie 8 pour l’action (0 : rien, 1 : attaque, 2 : dash)
        int action = Mathf.RoundToInt(outputs[8]);

        if (action == 1 && Vector2.Distance(transform.position, FindNearestTarget().position) <= attackRange)
        {
            PerformAttack();
        }
        else if (action == 2)
        {
            PerformDash();
        }
    }

    void PerformAttack()
    {
        // Logique pour l'attaque
        Debug.Log("L'IA attaque !");
        // Inclure les récompenses dans le système d'entraînement du réseau en fonction de la réussite
    }

    void PerformDash()
    {
        // Logique pour le dash dans la direction actuelle
        Debug.Log("L'IA dash !");
        rb.AddForce(moveDirection * moveSpeed * 5, ForceMode2D.Impulse); // Dash rapide
    }

    Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float closestDistance = detectionRadius;

        foreach (Transform entity in allEntities)
        {
            if (entity == this.transform) continue;

            float distance = Vector2.Distance(transform.position, entity.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestTarget = entity;
            }
        }
        return nearestTarget;
    }
}
