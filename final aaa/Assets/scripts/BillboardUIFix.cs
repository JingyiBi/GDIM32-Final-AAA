using UnityEngine;

public class BillboardUIFix : MonoBehaviour
{
    private Transform cam;
    public float verticalOffset = 2.8f; 

    void Start() => cam = Camera.main.transform;

    void LateUpdate()
    {
       
        transform.LookAt(transform.position + cam.forward);

        
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + Vector3.up * verticalOffset;
        }

        
        transform.position += (cam.position - transform.position).normalized * 0.1f;
    }
}