using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite[] animationSprites;
    public float animationTime = 0.25f;
    private int animationFrame;

    public bool loop = true;
    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    /// <summary>
    ///  次フレームのアニメーションを作成
    /// </summary>
    private void NextFrame()
    {
        animationFrame++;
        if(loop && animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }
        if (idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        else if(animationFrame >= 0 && animationFrame < animationSprites.Length)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}
