using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public float speed = 5.0f;
    public bool hasPowerUp = false;
    private float powerupStrength = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
      playerRb = GetComponent<Rigidbody>();  
      focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PowerUp")) {
            hasPowerUp = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine() {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp) {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerUp);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}
