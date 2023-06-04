using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreePopulator : MonoBehaviour
{
    GameObject treeOriginal;

    // Start is called before the first frame update
    void Start()
    {
        treeOriginal = GameObject.Find("TreeOriginal");

        for (float x = -50f; x <= 50f; x += 8f)
        {
            createTree(x);
        }

        GameObject.Destroy(treeOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createTree(float xPosition)
    {
        const float probabilityOfFrontTree = 0.3f;
        bool backOrFront = Random.Range(0f, 1f) > probabilityOfFrontTree;

        GameObject treeClone = Instantiate(treeOriginal, transform);
        treeClone.name = "Tree" + xPosition.ToString();
        treeClone.transform.localPosition = new Vector3(
            xPosition,
            treeOriginal.transform.localPosition.y - (backOrFront ? 0f : 0.2f),
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
}
