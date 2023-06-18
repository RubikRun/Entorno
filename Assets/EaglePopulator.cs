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

        fillRectWithEagles(-44f, 13f, 69f, 61f, 18);
        fillRectWithEagles(90f, 29f, 143f, 61f, 6);
        fillRectWithEagles(148f, 40f, 229f, 61f, 8);
        fillRectWithEagles(153f, 6f, 195f, 24f, 6);
        fillRectWithEagles(-46f, -49f, 28f, -7f, 10);

        GameObject.Destroy(eagleOriginal);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void createEagle(float x, float y)
    {
        GameObject eagleClone = Instantiate(eagleOriginal, transform);
        eagleClone.name = "Eagle" + x.ToString() + y.ToString();
        eagleClone.transform.localPosition = new Vector3(x, y, 0f);
    }

    void fillRectWithEagles(float xMin, float yMin, float xMax, float yMax, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float xDelta = Random.Range(0f, xMax - xMin);
            float yDelta = Random.Range(0f, yMax - yMin);
            float x = xMin + xDelta;
            float y = yMin + yDelta;
            createEagle(x, y);
        }
    }
}
