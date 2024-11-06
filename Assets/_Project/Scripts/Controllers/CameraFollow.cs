using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // La r�f�rence au personnage
    public float smoothSpeed = 0.05f; // Vitesse de lissage
    public Vector3 offset; // D�calage de la cam�ra

    private void LateUpdate()
    {
        if (target == null) return;

        // Cr�e une position cible bas�e uniquement sur les axes X et Y
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Applique un effet de lissage pour que la cam�ra suive progressivement le personnage
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Assigne la position liss�e
        transform.position = smoothedPosition;
    }
}
