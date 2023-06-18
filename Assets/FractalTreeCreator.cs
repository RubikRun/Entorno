using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FractalTreeCreator : MonoBehaviour
{
    public Sprite branchSprite;
    Bounds branchBounds;

    public int depthMax = 10;
    public float angle = 0.7f;
    public float ratio = 0.8f;
    public float height = 3f;
    public float thickness = 1f;

    private class Tree
    {
        public Tree(Vector3 A, Vector3 B, Tree leftChild, Tree rightChild)
        {
            this.A = A;
            this.B = B;
            this.leftChild = leftChild;
            this.rightChild = rightChild;
        }

        public Vector3 A;
        public Vector3 B;
        public Tree leftChild;
        public Tree rightChild;
    };

    private GameObject[] branches;
    private int branchesCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        branchBounds = branchSprite.bounds;
        branches = new GameObject[PowInt(2, depthMax + 1) - 1];

        Vector3 A = Vector3.zero;
        Vector3 B = new Vector3(0f, height, 0f);

        Tree tree = createTree(A, B, 0);
        createBranches(tree);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Tree createTree(Vector3 A, Vector3 B, int depth)
    {
        if (depth > depthMax)
        {
            return null;
        }

        Vector3 leftChildA = B;
        Vector3 leftChildB = new Vector3(
            B.x + (B.x - A.x) * ratio * Mathf.Cos(angle) + (B.y - A.y) * ratio * Mathf.Sin(angle),
            B.y + (B.y - A.y) * ratio * Mathf.Cos(angle) + (A.x - B.x) * ratio * Mathf.Sin(angle)
        );
        Tree leftChild = createTree(leftChildA, leftChildB, depth + 1);
        Vector3 rightChildA = B;
        Vector3 rightChildB = new Vector3(
            B.x + (B.x - A.x) * ratio * Mathf.Cos(angle) + (A.y - B.y) * ratio * Mathf.Sin(angle),
            B.y + (B.y - A.y) * ratio * Mathf.Cos(angle) + (B.x - A.x) * ratio * Mathf.Sin(angle)
        );
        Tree rightChild = createTree(rightChildA, rightChildB, depth + 1);

        Tree tree = new Tree(A, B, leftChild, rightChild);
        return tree;
    }

    private void createBranches(Tree tree)
    {
        GameObject branch = createBranch(tree.A, tree.B);
        branches[branchesCount++] = branch;
        if (tree.leftChild != null)
        {
            createBranches(tree.leftChild);
        }
        if (tree.rightChild != null)
        {
            createBranches(tree.rightChild);
        }
    }

    private GameObject createBranch(Vector3 A, Vector3 B)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan2(B.y - A.y, B.x - A.x);
        Vector3 midPoint = (A + B) * 0.5f;

        GameObject branch = new GameObject("Branch" + branchesCount.ToString());
        branch.transform.parent = transform;
        branch.transform.localPosition = midPoint;
        branch.transform.localRotation = Quaternion.identity;
        branch.transform.SetLocalPositionAndRotation(midPoint, Quaternion.Euler(0f, 0f, angle));
        float abDist = (A - B).magnitude;
        float branchScale = abDist / branchBounds.size.x;
        branch.transform.localScale = new Vector3(branchScale, branchScale * thickness, branchScale);

        SpriteRenderer spriteRenderer = branch.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = branchSprite;
        spriteRenderer.sortingLayerName = "BackEnv";

        return branch;
    }

    private int PowInt(int x, int p)
    {
        if (p == 0)
        {
            return 1;
        }
        if (p == 1)
        {
            return x;
        }
        int r = x;
        while (p > 1)
        {
            r *= x;
            p--;
        }
        return r;
    }
}
