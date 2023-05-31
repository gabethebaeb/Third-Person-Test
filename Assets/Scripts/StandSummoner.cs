using UnityEngine;

public class StandSummoner : MonoBehaviour
{
    public GameObject standPrefab; // The prefab for the stand
    public GameObject standInstance; // The currently summoned stand
    public bool isSummoned;
    public Transform player;
    public Vector3 summonPosition = new Vector3(-1, 1.5f, -1.5f);
    

    void Start()
    {
        player = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (standInstance == null)
            {
                // Summon the stand
                standInstance = Instantiate(standPrefab, transform.position, Quaternion.identity);
                isSummoned = true;
                // Set the stand's parent to the player
                standInstance.transform.parent = transform;
                // Position the stand behind the player
                standInstance.transform.localPosition = summonPosition;
                

            }
            else
            {
                // Despawn the stand
                Destroy(standInstance);
                standInstance = null;
                isSummoned = false;
            }
        }
        if (standInstance != null)
        {
            standInstance.transform.rotation = player.transform.rotation;
        }
    }
}