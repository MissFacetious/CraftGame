using UnityEngine;
using System.Collections;
using System;
public class Collectible : MonoBehaviour
{
    private Rigidbody rb;
    [Range(0f, 100f)]
    public float rotateSpeed = 35f;
    private float rotationAngle = 0f;
    public Recipes.RecipeEnum recipe;

    protected float emitAnim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // moved AddForce() to CollectibleEmitter.cs 
        //rb.AddForce(Vector3.up, ForceMode.Impulse);
        transform.Rotate(Vector3.up * 4f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }
    }

    void OnEnable()
    {
        Debug.Log($"{name} is enabled.");
        // when object pooling, use OnEnable instead of Awake/Start
    }

    public void Collect(GameObject collector)
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<Collider>());

        transform.parent = collector.transform;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        gameObject.GetComponent<AudioSource>().Play();
        ParticleSystem system = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        system.gameObject.SetActive(true);
        system.Play();

        GetComponent<Animator>().SetTrigger("collect");
    }

    public void Done()
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.inertiaTensorRotation = new Quaternion(0.01f, 0.01f, 0.01f, 1f);
            rb.AddTorque(-rb.angularVelocity);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            rb.Sleep();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            rotationAngle += rotateSpeed * Time.deltaTime;
            rb.MoveRotation(Quaternion.Euler(0f, rotationAngle, 0f));
        }
    }
}
