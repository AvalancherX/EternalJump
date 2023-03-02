using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    private Vector2 playerLastPos;
    [SerializeField] private float notOnPlatformTimer = 0f;
    private int jumpedCounter = 0;
    private float startingHeight;


    private Camera cam;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Movement();
        ManageCameraPosition();
    }

    void Movement()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * _moveSpeed);
            sr.flipX = true;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
            sr.flipX = false;
        }

        if (rb.velocity.y < 0f)
        {
            notOnPlatformTimer += Time.deltaTime;
            jumpedCounter = 0;
        }

        if (notOnPlatformTimer > 1.75f)
        {
            
            gameObject.SetActive(false);
        }

        anim.SetFloat("VelocityY", rb.velocity.y);

    }

    void ManageCameraPosition()  //Stopping of camera going down NEEDS TO BE FIXED//
    {
        Vector2 temp = cam.transform.position;
        // if (rb.velocity.y > 0f)
        //{                                        
        /*  startingHeight = cam.transform.position.y;
              float newCameraHeight = startingHeight + (gameObject.transform.position.y - cam.transform.position.y);
              Vector3 temp = new Vector3(0f, newCameraHeight, -3f);
              cam.transform.position = temp;          */
        temp.y += (this.gameObject.transform.position - cam.transform.position).y;// - cam.transform.position.y;
        cam.transform.position = new Vector3(0f, temp.y, -3f);
        // }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && jumpedCounter <1)
        {
            notOnPlatformTimer = 0f;
            playerLastPos = gameObject.transform.position;
            rb.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
            ++jumpedCounter;
            

        }
    }


}
