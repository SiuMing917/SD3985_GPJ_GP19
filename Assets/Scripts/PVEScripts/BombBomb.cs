using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombBomb : MonoBehaviour
{
    public float damage;

    [Header("Bomb")]
    public float bombFuseTime = 3f;
    public AnimationCurve curve;
    Coroutine coroutine;

    [Header("Explosion")]
    public BombExplosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public GameObject destructibleObjects;

    private void Awake()
    {
        explosionRadius = GameManager.PVEInstance.player1.GetComponent<BombController>().explosionRadius;
    }

    public void PlaceBombActive(Vector3 playPos)
    {
        coroutine = StartCoroutine(PlaceBomb(playPos));
    }

    public void ThrowBombActive(Vector3 playPos, Vector3 targetPos)
    {
        coroutine = StartCoroutine(ThrowBomb(playPos, targetPos));
    }


    private IEnumerator PlaceBomb(Vector3 playPos)
    {
        Vector2 position = playPos;

        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        transform.position = position;

        GameObject bomb = this.gameObject;
        GameManager.PVEInstance.player1.GetComponent<BombController>().bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //position2 = bomb.transform.position;
        //position2.x = Mathf.Round(position.x+1);
        //position2.y = Mathf.Round(position.y);

        NormalExplosion(position, bomb);
    }

    private IEnumerator ThrowBomb(Vector3 playPos, Vector3 position)
    {
        GameObject bomb = this.gameObject;
        transform.position = playPos;
        bomb.GetComponent<Renderer>().sortingOrder = 10;
        bomb.GetComponent<Collider2D>().enabled = false;

        GameObject bombpos = Instantiate(this.gameObject, position, Quaternion.identity);
        Color customColor = new Color(1f, 0f, 0f, 0.3f);
        bombpos.GetComponent<Renderer>().material.SetColor("_Color", customColor);
        bombpos.GetComponent<Collider2D>().enabled = false;

        Vector3 direction = (position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, position);
        float speed = distance / bombFuseTime;
        float startTime = Time.time;

        List<Vector3> bombPath = new List<Vector3>(); // List to store bomb's path

        Vector3 initialPosition = bomb.transform.position;
        Vector3 peakPosition = (initialPosition + position) / 2f;
        peakPosition.y += 2f; // Adjust the height for parabolic trajectory

        var timePast = 0f;
        var maxbombhegiht = 7f;
        while (timePast < bombFuseTime)
        {
            timePast += Time.deltaTime;

            var linearTime = timePast / bombFuseTime; //time 0 to 3s
            var heightTime = curve.Evaluate(linearTime); //value from curve

            var height = Mathf.Lerp(0f, maxbombhegiht, heightTime);//clamped between the max height and 0

            bomb.transform.position = Vector3.Lerp(initialPosition, position, linearTime) + new Vector3(0f, height, 0f);//adding values on y axis

            yield return null;
        }


        Destroy(bombpos);
        bomb.GetComponent<Collider2D>().enabled = true;
        bomb.GetComponent<Renderer>().sortingOrder = 3;

        BombExplosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        GameManager.PVEInstance.player1.GetComponent<BombController>().bombsRemaining++;
    }

    private void NormalExplosion(Vector2 position, GameObject bomb)
    {

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


        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            //Vector2 mask = position + 1f;
            //ClearDestructible(position);
            // ClearDestructible(mask);
            ClearDestructibleObject(position);
            return;
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
            NormalExplosion(other.transform.position, this.gameObject);
        }
    }
}
