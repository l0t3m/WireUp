using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BlockSection
{
    StraightSection,
    LeftCornerSection,
    TSection,
    RightCornerSection,
}

[RequireComponent(typeof(Collider))]
public class DragAndDrop : MonoBehaviour
{
    public event Action<DragAndDrop> OnUnsnap;

    [SerializeField] Camera currentCamera;
    [SerializeField] public BlockSection BlockType;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isDragging = false;
    private bool isSnapped = false;
    private const int dragYLevel = 3;
    private float dragZ;

    public int MaxOfType = 0;

    private bool isRotated = false;


    private void Start()
    {
        originalPosition = transform.position;
    }
    private void OnMouseDown()
    {
        if (!isSnapped)
            isDragging = true;

    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mouseWorld = GetMouseWorldPosition();
            Vector3 targetPosition = mouseWorld;
            transform.position = new Vector3(targetPosition.x, dragYLevel, targetPosition.z);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (isSnapped) return;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 10))
        {
            if (hit.transform.gameObject.tag != "GridItem")
                ResetBlock();            
        }
        else
            ResetBlock();
    }  

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && isSnapped)
        {
            transform.Rotate(new Vector3(0, isRotated ? -20 : 20, 0));
            isRotated = !isRotated;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || (collision.gameObject.tag == "BlockPiece") && !isSnapped)
            ResetPosition();
    }

    // Methods:
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        Plane dragPlane = new Plane(Vector3.up, new Vector3(0, dragYLevel, 0)); 

        if (dragPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return transform.position;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    public void DoSnap()
    {
        isSnapped = true;
        MaxOfType--;
        if (MaxOfType > 0)
            Instantiate(this, originalPosition, originalRotation);
    }

    /*public void DoUnsnap()
    {
        isSnapped = false;
        MaxOfType++;
    }*/

    private void ResetBlock()
    {
        OnUnsnap?.Invoke(this);
        isDragging = false;
        ResetPosition();
    }
}
