using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 direction = Vector3.right;
    public int size = 1;

    private Vector3 leftEdge;
    private Vector3 rightEdge;

    private void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            Vector3 position = this.transform.position;
            position.x = leftEdge.x - size;
            this.transform.position = position;
        }
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 position = this.transform.position;
            position.x = rightEdge.x + size;
            this.transform.position = position;
        }
        else
        {
            transform.Translate(this.direction * this.speed * Time.deltaTime);
        }
    }

}
