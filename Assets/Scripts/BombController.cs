using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    public int bombsRemaining;
    public AnimationCurve curve;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;
    public GameObject destructibleObjects;

    [Header("SkillTest")]
    public GameObject arrowPrefab;
    public GameObject spikePrefab;
    public GameObject gokuPrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            Vector2 position = transform.position;
            Instantiate(bombPrefab).GetComponent<Bomb>().PlaceBombActive(position);
        }

        if (bombsRemaining > 0 && Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("Rocket").gameObject.activeSelf) // 按下鼠標左鍵
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            bombsRemaining--;
            Instantiate(bombPrefab).GetComponent<Bomb>().ThrowBombActive(transform.position, clickPosition);
            //StartCoroutine(ThrowBomb(clickPosition)); // 執行放置炸彈到點擊位置的方法
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("SpikeArrow").gameObject.activeSelf) // 按下鼠標左鍵
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            StartCoroutine(ShootSpike(clickPosition));
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        //Vector2 position2 = transform.position;

        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //position2 = bomb.transform.position;
        //position2.x = Mathf.Round(position.x+1);
        //position2.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        //Explosion explosion2 = Instantiate(explosionPrefab, position2, Quaternion.identity);
        //explosion2.SetActiveRenderer(explosion2.start);
        //explosion2.DestroyAfter(explosionDuration);
        //Explode(position+Vector2.right, Vector2.up, explosionRadius);
        //Explode(position+Vector2.right, Vector2.down, explosionRadius);
        //Explode(position2, Vector2.left, explosionRadius);
        //Explode(position2, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
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
            ClearDestructible(position);
            // ClearDestructible(mask);
            ClearDestructibleObject(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
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

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }


    private IEnumerator ThrowBomb(Vector3 position)
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<Renderer>().sortingOrder = 10;
        bomb.GetComponent<Collider2D>().enabled = false;

        GameObject bombpos = Instantiate(bombPrefab, position, Quaternion.identity);
        Color customColor = new Color(1f, 0f, 0f, 0.3f);
        bombpos.GetComponent<Renderer>().material.SetColor("_Color", customColor);
        bombpos.GetComponent<Collider2D>().enabled = false;

        bombsRemaining--;

        Vector3 direction = (position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, position);
        float speed = distance / bombFuseTime;
        float startTime = Time.time;

        List<Vector3> bombPath = new List<Vector3>(); // List to store bomb's path

        Vector3 initialPosition = bomb.transform.position;
        Vector3 peakPosition = (initialPosition + position) / 2f;
        peakPosition.y += 2f; // Adjust the height for parabolic trajectory

        /* while (Time.time < startTime + bombFuseTime)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float journeyFraction = distanceCovered / distance;
            bombPath.Add(Vector3.Lerp(initialPosition, position, journeyFraction));
        
            Vector3 distanceToPeak = peakPosition - initialPosition;
            Vector3 distanceToTarget = position - peakPosition;
        
            if (journeyFraction < 0.5f)
            {
                bomb.transform.position = initialPosition + distanceToPeak * (journeyFraction / 0.5f);
            }
            else
            {
                bomb.transform.position = peakPosition + distanceToTarget * ((journeyFraction - 0.5f) / 0.5f);
            }

            yield return null;
        } */


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

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    /* public IEnumerator Curve(Vector3 start,V finish)
    {
        var timePast = 0f;

        while(timePast < bombFuseTime)
        {
            timePast += Time.deltaTime;

            var linearTime = timePast / bombFuseTime; //time 0 to 3s
            var heightTime = curve.Evaluate(linearTime);//value from curve

            var height = Mathf.Lerp(0f,3.0f,heightTime);//clamped between the max height and 0

            transform.position = Vector3.Lerp(start, finish, linearTime) + new Vector3(0f, height, 0f);//adding values on y axis

            yield return null;
        }

    } */

    private IEnumerator ShootSpike(Vector3 position)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<Renderer>().sortingOrder = 10;
        arrow.GetComponent<Collider2D>().enabled = false;

        GameObject spike = Instantiate(spikePrefab, position, Quaternion.identity);
        Color customColor = new Color(1f, 0f, 0f, 0.3f);
        spike.GetComponent<Renderer>().material.SetColor("_Color", customColor);
        spike.GetComponent<Collider2D>().enabled = false;

        Vector3 direction = (position - transform.position).normalized;
        arrow.transform.right = direction; // Rotate arrow towards player position

        float distance = Vector3.Distance(transform.position, position);
        float speed = distance / bombFuseTime;
        float startTime = Time.time;

        while (Time.time < startTime + bombFuseTime)
        {
            arrow.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }

        spike.GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
        spike.GetComponent<Collider2D>().enabled = true;

        Destroy(arrow);
    }

}
