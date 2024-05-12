using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Kamehameha : MonoBehaviour
{
    [Header("Explosion")]
    public BombExplosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Kamehameha")]
    public AnimationCurve curve;
    Coroutine coroutine;
    public GameObject player;
    private Vector3 playerStartPosition;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public GameObject destructibleObjects;

    private bool kamehamehaActive = false;

    public float skillcd = 0f;

    private void Awake()
    {
        explosionRadius = GameManager.PVEInstance.player1.GetComponent<BombController>().explosionRadius;
    }

    public void KamehamehaActive(Vector3 playPos)
    {
        playerStartPosition = player.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        UseKamehameha(playPos);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !kamehamehaActive && skillcd <= 0f)
        {
            Vector2 position = player.transform.position;
            KamehamehaActive(position);
            skillcd = 5f;
        }
        if (skillcd >= 0f)
            skillcd -= Time.deltaTime;
    }

    private void UseKamehameha(Vector3 playPos)
    {
        kamehamehaActive = true;
        Vector2 position = playPos;
        //position += Vector2.right;

        if (player.transform.Find("Movement").Find("Right").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.right, 15);
        }else if(player.transform.Find("Movement").Find("Left").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.left);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.left, 15);
        }
        else if (player.transform.Find("Movement").Find("Up").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.up);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.up, 15);
        }
        else if (player.transform.Find("Movement").Find("Down").GetComponent<SpriteRenderer>().enabled)
        {
            BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.SetDirection(Vector2.down);
            explosion.DestroyAfter(explosionDuration);
            Explode(position, Vector2.down, 15);
        }
        StartCoroutine(EnablePlayerMovementAfterKamehameha());
    }

    private IEnumerator EnablePlayerMovementAfterKamehameha()
    {
        yield return new WaitForSeconds(explosionDuration);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.transform.position = playerStartPosition;
        kamehamehaActive = false;
    }

    private void NormalExplosion(Vector2 position, GameObject bomb){

        BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        int PlayerExplosionRadius = GameManager.PVEInstance.player1.GetComponent<BombController>().explosionRadius;
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        Explode(position, Vector2.up, PlayerExplosionRadius);
        Explode(position, Vector2.down, PlayerExplosionRadius);
        Explode(position, Vector2.left, PlayerExplosionRadius);
        Explode(position, Vector2.right, PlayerExplosionRadius);

        Destroy(this.gameObject);
        GameManager.PVEInstance.player1.GetComponent<BombController>().bombsRemaining++;
        coroutine = null;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        Collider2D hitCollider = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask);

        if (hitCollider != null)
        {
            //ClearDestructible(position);
            ClearDestructibleObject(position);
        }

        BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Tilemap t = GameManager.PVEInstance.destructibleTiles;
        Vector3Int cell = t.WorldToCell(position);
        TileBase tile = t.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            t.SetTile(cell, null);
        }
    }

    private void ClearDestructibleObject(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, Vector2.one, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Destructible"))
            {
                Instantiate(destructiblePrefab, position, Quaternion.identity);
                Destroy(collider.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (coroutine != null && other.CompareTag("Spike"))
        {
            StopCoroutine(coroutine);
            coroutine = null;
            NormalExplosion(other.transform.position,this.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hitted");
        }
    }
}
