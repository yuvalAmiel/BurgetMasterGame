using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    void Start()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,
                                                desiredPosition,
                                                smoothSpeed * Time.deltaTime);
        transform.position = desiredPosition;

        transform.LookAt(target);
    }

    void FixedUpdate()
    {
        Debug.Log("in fixed update cam");
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,
                                                desiredPosition,
                                                smoothSpeed * Time.deltaTime);
        transform.position = desiredPosition;

        transform.LookAt(target);

    }
}
