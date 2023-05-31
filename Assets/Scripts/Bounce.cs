using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Transform player; // Add this line
    public float bounceHeight = 2f;
    public float bounceSpeed = 2f;
    private float startY;
    private float currentY;

    void Start()
    {
        player = GetComponentInParent<StandSummoner>().player; // Assuming the StandSummoner script is attached to the parent object
        startY = transform.position.y - player.position.y;
        currentY = startY;
    }

    void Update()
    {
        // Modify the calculation of currentY to be relative to the player's y-position
        currentY = player.position.y + startY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
