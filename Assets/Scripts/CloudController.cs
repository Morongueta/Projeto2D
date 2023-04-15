using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gatto;
using Gatto.Utils;

public class CloudController : MonoBehaviour
{
    [SerializeField] private Sprite[] cloudSprites;
    [SerializeField] private bool debug;
    [SerializeField] private float minCloudSpeed;
    [SerializeField] private float maxCloudSpeed;

    [SerializeField] private float cloudStartX;
    [SerializeField] private float cloudEndX;
    [SerializeField] private float minCloudHeight;
    [SerializeField] private float maxCloudHeight;

    [SerializeField] private float minCloudTick;
    [SerializeField] private float maxCloudTick;

    [SerializeField] private float minCloudScale;
    [SerializeField] private float maxCloudScale;


    private float cloudTick;

    private void Start()
    {
        cloudTick = Random.Range(minCloudTick, maxCloudTick);

        int randomStartClouds = Random.Range(5, 8);

        for (int i = 0; i < randomStartClouds; i++)
        {
            GameObject cloud = PoolObject.Instantiate("CloudObject", new Vector3(Random.Range(cloudStartX, cloudEndX), Random.Range(minCloudHeight, maxCloudHeight), 0f) + transform.position, Quaternion.identity);
            cloud.LeanMoveX(cloudEndX + transform.position.x, Random.Range(minCloudSpeed, maxCloudSpeed)).setOnComplete(() =>
            {
                PoolObject.Destroy(cloud);
            });

            cloud.transform.localScale = Vector3.one * Random.Range(minCloudScale, maxCloudScale);

            cloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        }
    }
    private void Update()
    {
        cloudTick -= Time.deltaTime;

        if(cloudTick <= 0f)
        {
            GameObject cloud = PoolObject.Instantiate("CloudObject", new Vector3(cloudStartX, Random.Range(minCloudHeight, maxCloudHeight), 0f) + transform.position, Quaternion.identity);
            cloud.LeanMoveX(cloudEndX + transform.position.x, Random.Range(minCloudSpeed, maxCloudSpeed)).setOnComplete(() =>
            {
                PoolObject.Destroy(cloud);
            });

            cloud.transform.localScale = Vector3.one * Random.Range(minCloudScale, maxCloudScale);

            cloudTick = Random.Range(minCloudTick, maxCloudTick);

            cloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        }
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Gizmos.DrawWireSphere(new Vector2(cloudStartX, maxCloudHeight) + (Vector2)transform.position, .1f);
        Gizmos.DrawWireSphere(new Vector2(cloudEndX, maxCloudHeight) + (Vector2)transform.position, .1f);

        Gizmos.DrawWireSphere(new Vector2(cloudStartX, minCloudHeight) + (Vector2)transform.position, .1f);
        Gizmos.DrawWireSphere(new Vector2(cloudStartX, maxCloudHeight) + (Vector2)transform.position, .1f);

        Gizmos.DrawLine(new Vector2(cloudStartX, maxCloudHeight) + (Vector2)transform.position, new Vector2(cloudEndX, maxCloudHeight) + (Vector2)transform.position);
    }
}
