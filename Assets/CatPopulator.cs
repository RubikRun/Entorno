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

        createCat(0f, 0f);
        createCat(172f, 0f);
        createCat(230f, 31f);
        createCat(52f, -40f);

        GameObject.Destroy(catOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createCat(float x, float y)
    {
        GameObject catClone = Instantiate(catOriginal, transform);
        catClone.name = "Cat" + x.ToString() + y.ToString();
        catClone.transform.localPosition = new Vector3(x, y, 0);
    }
}
