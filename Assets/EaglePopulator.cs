using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglePopulator : MonoBehaviour
{
    GameObject eagleOriginal;

    // Start is called before the first frame update
    void Start()
    {
        eagleOriginal = GameObject.Find("EagleOriginal");

        const float distBetweenEagles = 24f;

        for (float x = distBetweenEagles; x <= 50f; x += distBetweenEagles)
        {
            createEagle(x);
        }
        for (float x = -50f; x <= -distBetweenEagles; x += distBetweenEagles)
        {
            createEagle(x);
        }

        GameObject.Destroy(eagleOriginal);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void createEagle(float xPosition)
    {
        GameObject eagleClone = Instantiate(eagleOriginal, transform);
        eagleClone.name = "Eagle" + xPosition.ToString();
        eagleClone.transform.localPosition = new Vector3(xPosition, eagleOriginal.transform.localPosition.y, 0);
    }
}
