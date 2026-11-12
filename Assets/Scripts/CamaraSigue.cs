using UnityEngine;

public class CamaraSigue : MonoBehaviour
{
    [SerializeField] Transform objetivo;
    float maxX;
    void Start() { maxX = transform.position.x; }
    void LateUpdate()
    {
        if (objetivo.position.x > maxX) maxX = objetivo.position.x;
        transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
    }
}