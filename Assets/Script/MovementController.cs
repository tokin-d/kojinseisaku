
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;

    public float speed = 5f;

    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// キー入力処理
    /// </summary>
    private void Update()
    {
        if(Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up);

        } 
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down);
        }
        else if(Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left);
        }
        else if(Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right);
        }
        else
        {
            SetDirection(Vector2.zero);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }

    /// <summary>
    /// 移動処理をrigidBody2Dで行う
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = speed * Time.fixedDeltaTime * direction;

        rb.MovePosition(position + translation);
    }

    /// <summary>
    /// 新しい方向をセットする
    /// </summary>
    /// <param name="newDirection"></param>
    private void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    /// <summary>
    /// 死亡シークエンス
    /// </summary>
    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        Invoke(nameof(OnDeathSequeceEnabled), 1.0f);
    }


    /// <summary>
    /// 死亡シークエンスが有効かどうか
    /// </summary>
    private void OnDeathSequeceEnabled()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CheckWinState();
    }
}
