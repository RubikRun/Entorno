using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreePopulator : MonoBehaviour
{
    GameObject treeOriginal;
    private Sprite treeSprite;

    // Start is called before the first frame update
    void Start()
    {
        treeOriginal = GameObject.Find("TreeOriginal");
        treeSprite = treeOriginal.GetComponent<SpriteRenderer>().sprite;

        createTree(60f, 2.75f);
        createTree(130f, 19.73f);

        fillHorizontalRange(34.26f, 165f, 189f, 24f, 10f);
        fillHorizontalRange(34.26f, 208f, 228f, 24f, 10f);
        fillHorizontalRange(23.78f, 207.4f, 237f, 16f, 6f);

        GameObject.Destroy(treeOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createTree(float x, float y)
    {
        const float probabilityOfFrontTree = 0.3f;
        bool backOrFront = Random.Range(0f, 1f) > probabilityOfFrontTree;

        GameObject treeClone = Instantiate(treeOriginal, transform);
        treeClone.name = "Tree" + x.ToString() + y.ToString();
        treeClone.transform.localPosition = new Vector3(
            x,
            y - (backOrFront ? 0f : 0.2f),
            0
        );

        SpriteRenderer spriteRenderer = treeClone.GetComponent<SpriteRenderer>();
        if (backOrFront)
        {
            spriteRenderer.sortingLayerName = "BackEnv";
        }
        else
        {
            spriteRenderer.sortingLayerName = "FrontEnv";
        }
    }

    private void fillHorizontalRange(float y, float xLeft, float xRight, float distCenter, float distAmp)
    {
        float x = xLeft;
        while (x < xRight)
        {
            createTree(x, y);
            float distDelta = Random.Range(-distAmp, +distAmp);
            x += distCenter + distDelta;
        }
    }
}
