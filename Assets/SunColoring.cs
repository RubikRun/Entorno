using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunColoring : MonoBehaviour
{
    [SerializeField]
    Sprite yellowSprite;

    [SerializeField]
    Sprite redSprite;

    [SerializeField]
    Sprite blueSprite;

    private SunOrbiting sunOrbiting;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sunOrbiting = GetComponent<SunOrbiting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sunOrbiting.days % 3 == 0)
        {
            spriteRenderer.sprite = blueSprite;
        }
        else if (sunOrbiting.days % 3 == 1)
        {
            spriteRenderer.sprite = yellowSprite;
        }
        else
        {
            spriteRenderer.sprite = redSprite;
        }
    }
}
