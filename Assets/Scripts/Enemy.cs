using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //[SerializeField] Transform player;
    public GameObject player;

    NavMeshAgent agent;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 3f;
    private float dashingCooldown = 3f;
    private Vector2 targetdestination;
    private float step;

    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update

    void Start()
    {
        targetdestination = new Vector2(0.0f, 0.0f);
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        step = dashingPower * Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        targetdestination = player.transform.position;

        if (isDashing)
        {
            return;
        }

        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if(distance <= 3f)
        {
            Vector3 targetdestination = player.transform.position;
            
            StartCoroutine(Dash());
        }
        //Debug.Log(targetdestination);

    }

    private IEnumerator Dash()
    {

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        //rb.velocity = new Vector2(player.transform.position.x * dashingPower, player.transform.position.y * dashingPower);
        rb.velocity = Vector2.MoveTowards(transform.position, targetdestination, step);

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }

}
