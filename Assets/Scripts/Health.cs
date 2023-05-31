using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Add this line at the top with other using statements

public class Health : MonoBehaviour
{
    [SerializeField]
    public float health;

    public GameObject HitTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.health = 10f;
    }

    public void TakeDamage(float damage)
    {
        //ShowHitText(damage);
        this.health -= damage;

        if (HitTextPrefab != null && health > 0)
        {
            ShowHitText(damage);
        }

        if (health <= 0)
        {
            ShowHitText(damage);
            Die();
        }
    }

    void ShowHitText(float damage)
    {
        GameObject hitTextGO = Instantiate(HitTextPrefab, transform.position, Quaternion.identity, transform);
        hitTextGO.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void Die()
    {
        //Debug.Log("I am Dead!");
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }
}