using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 direction;
    private bool isInConversationRange;
    private Collider2D conversator;

    // Start is called before the first frame update
    void Start()
    {
        isInConversationRange = false;
        direction = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput();
        Move();
    }

    //Movement calculations
    public void Move()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    //Controller inputs
    private void GetInput()
    {
        direction = new Vector2(0,0);
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("REGISTERED");
            conversator.GetComponentInParent<InkHandler>().StartConversation();
            Debug.Log("started convo");
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("enteredtrigger");
        conversator = other;
        isInConversationRange = true;
        
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        other.GetComponentInParent<InkHandler>().EndConversation();
        conversator = null;
        isInConversationRange = false;
        Debug.Log("finished convo");
    }
}
