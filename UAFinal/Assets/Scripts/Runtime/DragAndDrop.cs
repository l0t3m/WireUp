using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DragAndDrop : MonoBehaviour
{
    public event Action<DragAndDrop> OnUnsnap;

    [SerializeField] Camera currentCamera;
    private Vector3 offset;
    private bool isDragging = false;
    public bool isSnapped = false;
    private int dragYLevel = 3;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
            transform.position = new Vector3(transform.position.x, dragYLevel, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        OnUnsnap?.Invoke(this);
        isDragging = false;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            ResetPosition();
    }

    public void DoSnap()
    {
        isSnapped = true;
    }

    public void DoUnsnap()
    {
        isSnapped = false;
    }

    // Methods:
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenMousePosition = Input.mousePosition;
        screenMousePosition.z = currentCamera.WorldToScreenPoint(transform.position).z;
        return currentCamera.ScreenToWorldPoint(screenMousePosition);
    }
}
