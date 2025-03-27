using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (isSnapped)
        {
            OnUnsnap?.Invoke(this);
        }
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
        isDragging = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 10))
        {
            if (hit.transform.gameObject.tag != "GridItem")
            {
                OnUnsnap?.Invoke(this);
                isDragging = false;
                ResetPosition();
            }
        }
        else
        {
            OnUnsnap?.Invoke(this);
            isDragging = false;
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || (collision.gameObject.tag == "BlockPiece") && !isSnapped)
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
