using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // La référence au personnage
    public float smoothSpeed = 0.05f; // Vitesse de lissage
    public Vector3 offset; // Décalage de la caméra

    private void LateUpdate()
    {
        if (target == null) return;

        // Crée une position cible basée uniquement sur les axes X et Y
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Applique un effet de lissage pour que la caméra suive progressivement le personnage
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Assigne la position lissée
        transform.position = smoothedPosition;
    }
}
