using System;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenTracker : MonoBehaviour
{
    [SerializeField] private Transform trackTo;
    [SerializeField] private Transform trackFrom;
    [SerializeField] private GameObject directionIndicator;

    private Camera cam;
    private float camHeight;
    private float camWidth;

    private void Awake()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize * 2;
        camWidth = cam.orthographicSize * 2 * ((float)Screen.width / (float)Screen.height);
    }

    private void Update()
    {
        SetIndicatorPosition();
    }

    private void SetIndicatorPosition()
    {
        Vector3 position = trackFrom.position + (Vector3)GetDirection().normalized * 30;
        float maxX = cam.transform.position.x + (camWidth / 2);
        float minX = cam.transform.position.x - (camWidth / 2);
        float maxY = cam.transform.position.y + (camHeight / 2);
        float minY = cam.transform.position.y - (camHeight / 2);

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        position.z = 0;

        if (trackTo.position.x < maxX && trackTo.position.x > minX && trackTo.position.y < maxY && trackTo.position.y > minY)
            directionIndicator.SetActive(false);
        else
            directionIndicator.SetActive(true);


        directionIndicator.transform.position = position;
    }

    private Vector2 GetDirection()
    {
        Vector2 dir = trackTo.position - trackFrom.position;

        return dir;
    }
}
