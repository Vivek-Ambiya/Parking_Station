using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ParkingZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color parkedColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;

    private void Reset()
    {
        // Ensure collider is trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ChangeColor(other, parkedColor);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ChangeColor(other, defaultColor);
    }

    private void ChangeColor(Collider player, Color color)
    {
        Renderer renderer = player.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}