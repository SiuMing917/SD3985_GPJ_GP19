using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class Bomb : MonoBehaviourPun
{
    #region
    /**
    [Header("Bomb")]
    public int radius;
    public float explodeTime = 3.0f;
    [HideInInspector]
    public int x, y;
    public int host;
    public bool isStatic = true;
    private GameManager gameManager;
    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    private BoxCollider2D boxCollider;
    private bool isExploded = false;
    public Vector2Int targetCoord;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        if (!isExploded && explodeTime <= 0)
        {
            Explode();
            isExploded = true;
        }

        explodeTime -= Time.deltaTime;

    }


    private void Explode()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            return;

        if (this.gameObject != null)
        {
            if (PhotonNetwork.IsConnected == false)
            {
                StartCoroutine(gameManager.BombExplode(this.gameObject));
            }
            else if (PhotonNetwork.IsConnected == true && PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(gameManager.BombExplode(this.gameObject));
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] results = new Collider2D[4];
        int total = Physics2D.OverlapCollider(boxCollider, new ContactFilter2D().NoFilter(), results);
        //Debug.Log("total:" + total);
        if (total == 0)
            boxCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            StartCoroutine(gameManager.BombExplode(this.gameObject));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("stay " + collision.GetComponent<Person>().NO);
        boxCollider.isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Menu.mode != 2)
            return;
        Person player = collision.gameObject.GetComponent<Person>();
        if (!player.canPushBomb)
            return;
        if (player.orientation == 0 && collision.transform.position.y > transform.position.y)
        {
            
            if (y + 1 < gameManager.yRow && gameManager.itemsType[x, y + 1] == GameManager.ItemType.EMPTY)
            {
                targetCoord = new Vector2Int(x, y + 1);
                gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
                gameManager.itemsObject[x, y] = null;
                isStatic = false;
                
                #region
                gameManager.bombRange[x, y].Remove(GetComponent<Bomb>());
                gameManager.explosionRange[x, y]--;
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y - i < 0 || gameManager.itemsType[x, y - i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y - i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y - i]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x - i < 0 || gameManager.itemsType[x - i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x - i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x - i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x + i >= gameManager.xColumn || gameManager.itemsType[x + i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x + i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x + i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y + i >= gameManager.yRow || gameManager.itemsType[x, y + i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y + i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y + i]--;
                }
                #endregion
            }
        }
        if (player.orientation == 1 && collision.transform.position.x > transform.position.x)
        {

            if (x - 1 >= 0 && gameManager.itemsType[x - 1, y] == GameManager.ItemType.EMPTY)
            {
                targetCoord = new Vector2Int(x - 1, y);
                gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
                gameManager.itemsObject[x, y] = null;
                isStatic = false;
                
                #region
                gameManager.bombRange[x, y].Remove(GetComponent<Bomb>());
                gameManager.explosionRange[x, y]--;
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y - i < 0 || gameManager.itemsType[x, y - i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y - i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y - i]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x - i < 0 || gameManager.itemsType[x - i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x - i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x - i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x + i >= gameManager.xColumn || gameManager.itemsType[x + i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x + i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x + i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y + i >= gameManager.yRow || gameManager.itemsType[x, y + i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y + i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y + i]--;
                }
                #endregion
            }
        }
        if (player.orientation == 2 && collision.transform.position.x < transform.position.x)
        {
            
            if (x + 1 < gameManager.xColumn && gameManager.itemsType[x + 1, y] == GameManager.ItemType.EMPTY)
            {
                targetCoord = new Vector2Int(x + 1, y);
                gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
                gameManager.itemsObject[x, y] = null;
                isStatic = false;
                
                #region
                gameManager.bombRange[x, y].Remove(GetComponent<Bomb>());
                gameManager.explosionRange[x, y]--;
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y - i < 0 || gameManager.itemsType[x, y - i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y - i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y - i]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x - i < 0 || gameManager.itemsType[x - i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x - i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x - i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x + i >= gameManager.xColumn || gameManager.itemsType[x + i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x + i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x + i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y + i >= gameManager.yRow || gameManager.itemsType[x, y + i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y + i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y + i]--;
                }
                #endregion
            }
        }
        if (player.orientation == 3 && collision.transform.position.y < transform.position.y)
        {
            
            if (y - 1 >= 0 && gameManager.itemsType[x, y - 1] == GameManager.ItemType.EMPTY)
            {
                targetCoord = new Vector2Int(x, y - 1);
                gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
                gameManager.itemsObject[x, y] = null;
                isStatic = false;
                
                #region
                gameManager.bombRange[x, y].Remove(GetComponent<Bomb>());
                gameManager.explosionRange[x, y]--;
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y - i < 0 || gameManager.itemsType[x, y - i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y - i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y - i]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x - i < 0 || gameManager.itemsType[x - i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x - i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x - i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (x + i >= gameManager.xColumn || gameManager.itemsType[x + i, y] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x + i, y].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x + i, y]--;
                }
                
                for (int i = 1; i <= radius; i++)
                {
                    if (y + i >= gameManager.yRow || gameManager.itemsType[x, y + i] == GameManager.ItemType.BARRIAR)
                        break;
                    gameManager.bombRange[x, y + i].Remove(GetComponent<Bomb>());
                    gameManager.explosionRange[x, y + i]--;
                }
                #endregion
            }
        }
    }
    **/
    #endregion

    //OLD CODE For Reference
    #region
    ///*
    public float damage;

    [Header("Bomb")]
    public float bombFuseTime = 3f;
    public AnimationCurve curve;
    Coroutine coroutine;
    private GameManager gameManager;
    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    public int radius;
    public float explodeTime = 3.0f;
    public int bombID;

    [HideInInspector]
    public int x, y;
    public int host;
    public bool isStatic = true;
    public bool isExploded = false;
    public bool isDestroyed = false;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public GameObject destructibleObjects;

    private void Awake(){
        gameManager = GameManager.Instance;
        //explosionRadius = GameManager.instance.player1.GetComponent<BombController>().explosionRadius;
    }

    private void Update()
    {
        //if (!isExploded && explodeTime <= 0)
        //{
        //    Explode();
        //    isExploded = true;
        //}

        //explodeTime -= Time.deltaTime;

    }

    private void Explode()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            return;

        if (this.gameObject != null)
        {
            if (PhotonNetwork.IsConnected == false)
            {
                StartCoroutine(gameManager.BombExplode(this.gameObject));
            }
            else if (PhotonNetwork.IsConnected == true && PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(gameManager.BombExplode(this.gameObject));
            }
        }

    }

    public void ThrowBombActive(Vector3 playPos, Vector3 targetPos)
    {
        //coroutine = StartCoroutine(ThrowBomb(playPos, targetPos));
    }



    //private IEnumerator ThrowBomb(Vector3 playPos, Vector3 position)
    //{
    //    GameObject bomb = this.gameObject;
    //    transform.position = playPos;
    //    bomb.GetComponent<Renderer>().sortingOrder = 10;
    //    bomb.GetComponent<Collider2D>().enabled = false;

    //    GameObject bombpos = Instantiate(this.gameObject, position, Quaternion.identity);
    //    Color customColor = new Color(1f, 0f, 0f, 0.3f);
    //    bombpos.GetComponent<Renderer>().material.SetColor("_Color", customColor);
    //    bombpos.GetComponent<Collider2D>().enabled = false;

    //    Vector3 direction = (position - transform.position).normalized;
    //    float distance = Vector3.Distance(transform.position, position);
    //    float speed = distance / bombFuseTime;
    //    float startTime = Time.time;

    //    List<Vector3> bombPath = new List<Vector3>(); // List to store bomb's path

    //    Vector3 initialPosition = bomb.transform.position;
    //    Vector3 peakPosition = (initialPosition + position) / 2f;
    //    peakPosition.y += 2f; // Adjust the height for parabolic trajectory

    //    var timePast = 0f;
    //    var maxbombhegiht = 7f;
    //    while (timePast < bombFuseTime)
    //    {
    //        timePast += Time.deltaTime;

    //        var linearTime = timePast / bombFuseTime; //time 0 to 3s
    //        var heightTime = curve.Evaluate(linearTime); //value from curve

    //        var height = Mathf.Lerp(0f, maxbombhegiht, heightTime);//clamped between the max height and 0

    //        bomb.transform.position = Vector3.Lerp(initialPosition, position, linearTime) + new Vector3(0f, height, 0f);//adding values on y axis

    //        yield return null;
    //    }


    //    Destroy(bombpos);
    //    bomb.GetComponent<Collider2D>().enabled = true;
    //    bomb.GetComponent<Renderer>().sortingOrder = 3;

    //    Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
    //    explosion.SetActiveRenderer(explosion.start);
    //    explosion.DestroyAfter(explosionDuration);
    //    Explode(position, Vector2.up, explosionRadius);
    //    Explode(position, Vector2.down, explosionRadius);
    //    Explode(position, Vector2.left, explosionRadius);
    //    Explode(position, Vector2.right, explosionRadius);

    //    Destroy(bomb);
    //    GameManager.instance.player1.GetComponent<BombController>().bombsRemaining++;
    //}


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
        //if (coroutine != null && other.CompareTag("Spike"))
        //{
        //    StopCoroutine(coroutine);
        //    coroutine = null;
        //}
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            //bombFuseTime = 0f;
            isExploded = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            //bombFuseTime = 0f;
            isExploded = true;
        }

    }

    private void OnDestroy()
    {
        if (this.isDestroyed == false)
            GameManager.roleList.GetT(host).GetComponent<Person>().bombNumber++;
    }

    //**/
    #endregion
}
