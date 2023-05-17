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
    [SerializeField] GameObject triceraBody;
    [SerializeField] GameObject triceraBabyBody;

    [SerializeField] int bodyAmount;

    List<GameObject> tRexBodies = new List<GameObject>();
    List<GameObject> raptorBodies = new List<GameObject>();
    List<GameObject> pteroBodies = new List<GameObject>();
    List<GameObject> pachyBodies = new List<GameObject>();
    List<GameObject> brontoBodies = new List<GameObject>();
    List<GameObject> ankyloBodies = new List<GameObject>();
    List<GameObject> triceraBodies = new List<GameObject>();
    List<GameObject> triceraBabyBodies = new List<GameObject>();
    void Start()
    {
        InitializeBodies(tRexBody, tRexBodies);
        InitializeBodies(raptorBody, raptorBodies);
        InitializeBodies(pteroBody, pteroBodies);
        InitializeBodies(pachyBody, pachyBodies);
        InitializeBodies(brontoBody, brontoBodies);
        InitializeBodies(ankyloBody, ankyloBodies);
        InitializeBodies(triceraBody, triceraBodies);
        InitializeBodies(triceraBabyBody, triceraBabyBodies);
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

            case EnemyType.Triceratops:
                if (triceraBodies.Count != 0)
                {
                    body = triceraBodies[0];
                    triceraBodies.Remove(body);
                }
                else body = Instantiate(triceraBody);
                break;

            case EnemyType.Triceratops_Baby:
                if (triceraBabyBodies.Count != 0)
                {
                    body = triceraBabyBodies[0];
                    triceraBabyBodies.Remove(body);
                }
                else body = Instantiate(triceraBabyBody);
                break;

            default:
                body = null;
                break;
        }
        body.transform.position = pos.position;
        body.transform.localScale = pos.localScale;
        body.transform.parent = this.transform;
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
            case EnemyType.Triceratops:
                triceraBodies.Add(body);
                break;
            case EnemyType.Triceratops_Baby:
                triceraBabyBodies.Add(body);
                break;

        }
    }
}
