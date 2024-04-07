using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask objectMask;

    public float sinTime;
    public float moveSpeed;

    public GameObject boxObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, objectMask);

        if(hit.collider != null && Input.GetKey(KeyCode.RightArrow)){

            boxObject = hit.collider.gameObject;
            Vector2 current = boxObject.GetComponent<Rigidbody2D>().position;
            Vector2 translation = Vector2.right * 1f * Time.fixedDeltaTime;
            Vector2 target = current + Vector2.right;

            if(current != target){
                sinTime += Time.fixedDeltaTime*moveSpeed;
                sinTime = Mathf.Clamp(sinTime,0,Mathf.PI);
                float ttime = evaluate(sinTime);
                boxObject.GetComponent<Rigidbody2D>().position = Vector2.Lerp(current,target,ttime);
            }

            if (boxObject.GetComponent<Rigidbody2D>().position != target){
                return;
            }

            Vector2 t = current;
            current = target;
            target = t;
            sinTime = 0;
        }
    }

    public float evaluate(float x){
        return 0.5f * Mathf.Sin(x-Mathf.PI/2f)+0.5f;
    }


}
