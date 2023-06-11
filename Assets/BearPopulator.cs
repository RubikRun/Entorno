using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearPopulator : MonoBehaviour
{
    GameObject bearOriginal;

    // Start is called before the first frame update
    void Start()
    {
        bearOriginal = GameObject.Find("BearOriginal");

        const float distBetweenBears = 34f;

        for (float x = distBetweenBears; x <= 50f; x += distBetweenBears)
        {
            createBear(x);
        }
        for (float x = -50f; x <= -distBetweenBears; x += distBetweenBears)
        {
            createBear(x);
        }

        GameObject.Destroy(bearOriginal);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void createBear(float xPosition)
    {
        GameObject bearClone = Instantiate(bearOriginal, transform);
        bearClone.name = "Bear" + xPosition.ToString();
        bearClone.transform.localPosition = new Vector3(xPosition, bearOriginal.transform.localPosition.y, 0);
    }
}
