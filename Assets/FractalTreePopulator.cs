using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTreePopulator : MonoBehaviour
{
    public Sprite branchSprite1;
    public Sprite branchSprite2;
    public Sprite branchSprite3;
    public Sprite branchSprite4;
    public Sprite branchSprite5;
    public Sprite branchSprite6;

    // Start is called before the first frame update
    void Start()
    {
        fillHorizontalRange(-3f, -50f + 5f, 50f - 5f, 20f, 8f);
        createTree(79f, 0.43f);
        createTree(99f, 13.68f);
        createTree(143.4f, 24.26f);
        fillHorizontalRange(30.26f, 158f, 189f, 30f, 15f);
        fillHorizontalRange(30.26f, 215f, 228f, 30f, 15f);
        fillHorizontalRange(-3.13f, 110f, 196f, 25f, 13f);
        createTree(41.4f, -41.86f);
        createTree(56f, -41.86f);
        createTree(84f, -43.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createTree(float x, float y)
    {
        GameObject tree = new GameObject("FractalTree" + x.ToString() + y.ToString());
        tree.transform.parent = transform;
        tree.transform.localPosition = new Vector3(x, y, 0f);

        FractalTreeCreator creator = tree.AddComponent<FractalTreeCreator>();
        creator.branchSprite = getRandomBranchSprite();
        creator.ratio = Random.Range(0.6f, 0.8f);
        creator.depthMax = Random.Range(5, 10);
        creator.angle = Random.Range(0.5f, 0.9f);
        creator.thickness = Random.Range(0.9f, 1.4f);
        creator.height = Random.Range(1.7f, 4.2f);
    }

    private Sprite getRandomBranchSprite()
    {
        int r = Random.Range(1, 6);
        if (r == 1) return branchSprite1;
        if (r == 2) return branchSprite2;
        if (r == 3) return branchSprite3;
        if (r == 4) return branchSprite4;
        if (r == 5) return branchSprite5;
        return branchSprite6;
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
