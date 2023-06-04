using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    PlayerAnimalForms playerAnimalForms;
    PlayerMovement playerMovement;

    private Transform target;
    private Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    private float smoothTime = 0.0f;

    public float tEnablingVertical = 0f;
    public float deltaEnablingVertical = 0.01f;

    private int enablingVertical = 0;
    private bool doVertical = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        playerAnimalForms = player.GetComponent<PlayerAnimalForms>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // When changing to a bird from a human, or vice versa, or when being a fish and going in and out of water,
        // the camera movement will change from following Player only horizontally to following them both horizontally and vertically.
        // When enabling the vertical following we want to do this smoothly and not directly jump there.
        // Because in when vertical is disabled Player is not in the center of the screen, and when enabled he is.
        // So just LERP-ing between vertical and non-vertical when changed.
        bool didVertical = doVertical;
        doVertical = playerAnimalForms.isBird || (playerAnimalForms.isFish && playerMovement.isInWater);
        if (doVertical != didVertical)
        {
            if (doVertical)
            {
                enablingVertical = 1;
            }
            else
            {
                enablingVertical = -1;
            }
        }
        if (enablingVertical != 0)
        {
            if (enablingVertical == 1 && tEnablingVertical >= 1f)
            {
                enablingVertical = 0;
                tEnablingVertical = 1f;
            }
            else if (enablingVertical == -1 && tEnablingVertical <= 0f)
            {
                enablingVertical = 0;
                tEnablingVertical = 0f;
            }
        }

        // Make the camera follow Player
        Vector3 targetPosition = new Vector3(
            target.position.x,
            Mathf.Lerp(3f, target.position.y, tEnablingVertical),
            -10f
        );
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (enablingVertical == 1)
        {
            tEnablingVertical += deltaEnablingVertical;
        }
        else if (enablingVertical == -1)
        {
            tEnablingVertical -= deltaEnablingVertical;
        }
    }
}
