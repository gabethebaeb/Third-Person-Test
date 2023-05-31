using UnityEngine;

public class abilityTeleport : StandAbility
{
    public GameObject markerPrefab;
    private GameObject markerInstance;
    private GameObject tempMarkerInstance;


    private bool markerActive = false;
    private bool cursorActive = false;

    public override void ActivateAbility()
    {
        if (isOnCooldown)
            return;

        if (!markerActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
            {
                markerInstance = Instantiate(markerPrefab, hit.point, Quaternion.identity);
                markerActive = true;
            }
        }
        else
        {
            player.position = markerInstance.transform.position;
            Destroy(markerInstance);
            markerActive = false;
            //StartCooldown();
        }
    }

    private float maxDistance = 100f;

    private void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.yellow);
        base.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            cursorActive = !cursorActive;
            Cursor.visible = cursorActive;
            Cursor.lockState = cursorActive ? CursorLockMode.None : CursorLockMode.Locked;

            if (!cursorActive && markerInstance)
            {
                Destroy(markerInstance);
                markerActive = false;
            }

            if (tempMarkerInstance)
            {
                Destroy(tempMarkerInstance);
            }
        }

        RaycastHit hit;
        if (cursorActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPosition;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = ray.GetPoint(maxDistance);
            }

            if (tempMarkerInstance == null)
            {
                tempMarkerInstance = Instantiate(markerPrefab, targetPosition, Quaternion.identity);
            }
            else
            {
                tempMarkerInstance.transform.position = targetPosition;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (tempMarkerInstance)
                {
                    Destroy(tempMarkerInstance);
                }

                if (!markerActive)
                {
                    markerInstance = Instantiate(markerPrefab, targetPosition, Quaternion.identity);
                    markerActive = true;
                    cursorActive = false;
                    Cursor.visible = cursorActive;
                    Cursor.lockState = cursorActive ? CursorLockMode.None : CursorLockMode.Locked;
                }
            }
        }
        else if (markerActive && Input.GetMouseButtonDown(1))
        {
            player.GetComponent<Collider>().enabled = false;
            Vector3 teleportPosition = markerInstance.transform.position + new Vector3(0, 0.5f, 0); // Add slight offset to avoid collider conflicts
            player.position = teleportPosition;
            player.GetComponent<Collider>().enabled = true;
            Destroy(markerInstance);
            markerActive = false;
            //StartCooldown();
        }
    }





    private void OnDisable()
    {
        if (markerInstance)
        {
            Destroy(markerInstance);
        }
        cursorActive = false;
        markerActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
