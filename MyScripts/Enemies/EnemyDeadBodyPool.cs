using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadBodyPool : MonoBehaviour
{
    [SerializeField] GameObject tRexBody;
    [SerializeField] GameObject raptorBody;
    [SerializeField] GameObject pteroBody;
    [SerializeField] GameObject pachyBody;
    [SerializeField] GameObject brontoBody;
    [SerializeField] GameObject ankyloBody;

    [SerializeField] int bodyAmount;

    [SerializeField] List<GameObject> tRexBodies = new List<GameObject>();
    [SerializeField] List<GameObject> raptorBodies = new List<GameObject>();
    [SerializeField] List<GameObject> pteroBodies = new List<GameObject>();
    [SerializeField] List<GameObject> pachyBodies = new List<GameObject>();
    [SerializeField] List<GameObject> brontoBodies = new List<GameObject>();
    [SerializeField] List<GameObject> ankyloBodies = new List<GameObject>();
    void Start()
    {
        InitializeBodies(tRexBody, tRexBodies);
        InitializeBodies(raptorBody, raptorBodies);
        InitializeBodies(pteroBody, pteroBodies);
        InitializeBodies(pachyBody, pachyBodies);
        InitializeBodies(brontoBody, brontoBodies);
        InitializeBodies(ankyloBody, ankyloBodies);
    }

    void InitializeBodies(GameObject body, List<GameObject> list)
    {
        for (int i = 0; i < bodyAmount; i++)
        {
            GameObject obj = Instantiate(body);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            list.Add(obj);
        }
    }

    public void GetBody(EnemyType type, Transform pos)
    {
        GameObject body;
        switch (type)
        {
            case EnemyType.T_Rex:
                if (tRexBodies.Count != 0)
                {
                    body = tRexBodies[0];
                    tRexBodies.Remove(body);
                }
                else body = Instantiate(tRexBody);
                break;

            case EnemyType.Raptor:
                if (raptorBodies.Count != 0)
                {
                    body = raptorBodies[0];
                    raptorBodies.Remove(body);
                }
                else body = Instantiate(raptorBody);
                break;

            case EnemyType.Pterodactyl:
                if (pteroBodies.Count != 0)
                {
                    body = pteroBodies[0];
                    pteroBodies.Remove(body);
                }
                else body = Instantiate(pteroBody);
                break;

            case EnemyType.Pachycephalosaurus:
                if (pachyBodies.Count != 0)
                {
                    body = pachyBodies[0];
                    pachyBodies.Remove(body);
                }
                else body = Instantiate(pachyBody);
                break;

            case EnemyType.Brontosaurus:
                if (brontoBodies.Count != 0)
                {
                    body = brontoBodies[0];
                    brontoBodies.Remove(body);
                }
                else body = Instantiate(brontoBody);
                break;

            case EnemyType.Ankylosaurus:
                if (ankyloBodies.Count != 0)
                {
                    body = ankyloBodies[0];
                    ankyloBodies.Remove(body);
                }
                else body = Instantiate(ankyloBody);
                break;

            default:
                body = null;
                break;
        }
        body.transform.position = pos.position;
        body.transform.localScale = pos.localScale;
        body.SetActive(true);
    }

    public void ReturnBody(GameObject body, EnemyType type)
    {
        switch (type)
        {
            case EnemyType.T_Rex:
                tRexBodies.Add(body);
                break;
            case EnemyType.Raptor:
                raptorBodies.Add(body);
                break;
            case EnemyType.Pterodactyl:
                pteroBodies.Add(body);
                break;
            case EnemyType.Pachycephalosaurus:
                pachyBodies.Add(body);
                break;
            case EnemyType.Ankylosaurus:
                ankyloBodies.Add(body);
                break;
        }
    }
}
