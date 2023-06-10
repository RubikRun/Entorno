using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SharkPopulator : MonoBehaviour
{
    GameObject sharkOriginal;

    GameObject water;

    // Start is called before the first frame update
    void Start()
    {
        sharkOriginal = GameObject.Find("SharkOriginal");
        water = GameObject.Find("Water");

        for (int i = 0; i < 10; i++)
        {
            createShark();
        }

        GameObject.Destroy(sharkOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createShark()
    {
        const float waterMargin = 0.1f;

        int watersCount = water.transform.childCount;
        int waterChildIdx = Random.Range(0, watersCount);
        GameObject waterChild = water.transform.GetChild(waterChildIdx).gameObject;
        SpriteRenderer waterChildSprite = waterChild.GetComponent<SpriteRenderer>();
        Bounds waterChildBounds = waterChildSprite.bounds;

        float xShark = Random.Range(
            waterChildBounds.min.x + waterChildBounds.size.x * waterMargin,
            waterChildBounds.max.x - waterChildBounds.size.x * waterMargin
        );
        float yShark = Random.Range(
            waterChildBounds.min.y + waterChildBounds.size.y * waterMargin,
            waterChildBounds.max.y - waterChildBounds.size.y * waterMargin
        );

        GameObject sharkClone = Instantiate(sharkOriginal, transform);
        sharkClone.name = "Shark_x_" + xShark.ToString() + "_y_" + yShark.ToString();
        sharkClone.transform.localPosition = new Vector3(xShark, yShark, 0);
    }
}
