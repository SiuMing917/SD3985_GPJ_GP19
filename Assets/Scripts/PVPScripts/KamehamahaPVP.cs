using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KamehamahaPVP : MonoBehaviourPun
{
    [Header("Explosion")]
    public BombExplosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;
    private GameManager gameManager;

    [Header("Kamehameha")]
    public AnimationCurve curve;
    Coroutine coroutine;
    public GameObject player;
    private Vector3 playerStartPosition;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public GameObject destructibleObjects;

    private bool kamehamehaActive = false;

    private void Awake()
    {
        //explosionRadius = GameManager.PVEInstance.player1.GetComponent<BombController>().explosionRadius;
        gameManager = GameManager.Instance;
    }

    public void KamehamehaActive(Vector3 playPos)
    {
        playerStartPosition = player.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        //UseKamehameha(playPos);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !kamehamehaActive)
        {
            Vector2 position = player.transform.position;
            KamehamehaActive(position);
        }
    }

    //[PunRPC]
    /**public void UseKamehameha(Vector3 playPos)
    {
        //kamehamehaActive = true;
        Vector2 position = playPos;
        //position += Vector2.right;

        if (player.transform.Find("Right").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.right, 15);
        }
        else if (player.transform.Find("Left").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.left);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.left, 15);
        }
        else if (player.transform.Find("Up").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.up);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.up, 15);
        }
        else if (player.transform.Find("Down").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.down);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.down, 15);
        }
        StartCoroutine(EnablePlayerMovementAfterKamehameha(player));
    }

    private IEnumerator EnablePlayerMovementAfterKamehameha(GameObject player)
    {
        yield return new WaitForSeconds(explosionDuration);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.transform.position = playerStartPosition;
        kamehamehaActive = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }**/
}
