using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;
    public float jumpForce;       
    public bool grounded = false; 
    public SpriteRenderer Sprite;   
    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sprite = gameObject.GetComponent<SpriteRenderer>();     

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
       
        Vector2 direction = new Vector2(horizontal,0);
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (horizontal < 0)
        {
            Sprite.flipX = true;
            anim.SetBool("walk", true);    
        }
        else if (horizontal > 0)
        {
            Sprite.flipX = false;
            anim.SetBool("walk", true);
        }


    }
     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = true;
            anim.SetBool("jump", false);

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            grounded = false;
            anim.SetBool("jump", true);

        }



    }
       
    



}
