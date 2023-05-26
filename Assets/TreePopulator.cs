using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreePopulator : MonoBehaviour
{
    [SerializeField]
    Sprite sprite;

    const float treeScale = 2f;

    // Start is called before the first frame update
    void Start()
    {
        for (float x = -50f; x <= 50f; x += 8f)
        {
            createTree(x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createTree(float xPosition)
    {
        bool backOrFront = Random.Range(0f, 1f) > 0.5f;

        GameObject treeObject = new GameObject("Tree" + xPosition.ToString());
        treeObject.transform.parent = transform;
        treeObject.transform.localScale = new Vector3(treeScale, treeScale, treeScale);
        treeObject.transform.localPosition = new Vector3(
            xPosition,
            sprite.bounds.size.y * treeScale * (backOrFront ? 0.98f : 0.93f) / 2f,
            0f
        );

        SpriteRenderer spriteRenderer = treeObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        if (backOrFront)
        {
            spriteRenderer.sortingLayerName = "BackEnv";
        }
        else
        {
            spriteRenderer.sortingLayerName = "FrontEnv";
        }
    }
}
