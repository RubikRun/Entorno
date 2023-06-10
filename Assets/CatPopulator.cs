using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPopulator : MonoBehaviour
{
    GameObject catOriginal;

    // Start is called before the first frame update
    void Start()
    {
        catOriginal = GameObject.Find("CatOriginal");

        const float distBetweenCats = 24f;

        for (float x = distBetweenCats; x <= 50f; x += distBetweenCats)
        {
            createCat(x);
        }
        for (float x = -50f; x <= -distBetweenCats; x += distBetweenCats)
        {
            createCat(x);
        }

        GameObject.Destroy(catOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createCat(float xPosition)
    {
        GameObject catClone = Instantiate(catOriginal, transform);
        catClone.name = "Cat" + xPosition.ToString();
        catClone.transform.localPosition = new Vector3(xPosition, catOriginal.transform.localPosition.y, 0);
    }
}
