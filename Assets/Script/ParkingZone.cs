using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ParkingZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color parkedColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;

    private Renderer zoneRenderer;

    private void Awake()
    {
        zoneRenderer = GetComponent<Renderer>();
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        zoneRenderer.material.color = parkedColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        zoneRenderer.material.color = defaultColor;
    }
}