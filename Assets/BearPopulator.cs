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

        fillHorizontalRange(0f, -50f + 8f, 50f - 8f, 32f, 8f);
        createBear(79.6f, 4.5f);
        createBear(126f, 17f);
        fillHorizontalRange(34f, 161f, 231f, 40f, 8f);
        fillHorizontalRange(24f, 208f, 231f, 32, 8f);
        fillHorizontalRange(1f, 110f, 195f, 45f, 16f);
        createBear(224f, -21f);
        createBear(82f, -38f);
        createBear(48f, -37f);

        GameObject.Destroy(bearOriginal);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void createBear(float x, float y)
    {
        GameObject bearClone = Instantiate(bearOriginal, transform);
        bearClone.name = "Bear" + x.ToString() + y.ToString();
        bearClone.transform.localPosition = new Vector3(x, y, 0);
    }

    private void fillHorizontalRange(float y, float xLeft, float xRight, float distCenter, float distAmp)
    {
        float x = xLeft;
        while (x < xRight)
        {
            createBear(x, y);
            float distDelta = Random.Range(-distAmp, +distAmp);
            x += distCenter + distDelta;
        }
    }
}
