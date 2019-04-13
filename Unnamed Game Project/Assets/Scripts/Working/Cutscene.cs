using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJandTARGET
{
    public GameObject obj;
    public Vector3 pos;

    public OBJandTARGET(GameObject _obj, Vector3 _pos)
    {
        obj = _obj;
        pos = _pos;
    }
}

public class Cutscene : MonoBehaviour
{
    public void MoveObject(GameObject obj, Vector3 pos)
    {
        OBJandTARGET objandTarget = new OBJandTARGET(obj, pos);

        StartCoroutine(MoveObjectCoroutine(objandTarget));
    }

    IEnumerator MoveObjectCoroutine(OBJandTARGET objpos)
    {
        bool isRight = false;

        if (objpos.obj.transform.position.x < objpos.pos.x)
        {
            isRight = true;
        }
        else if (objpos.obj.transform.position.x > objpos.pos.x)
        {
            isRight = false;
        }

        switch(isRight)
        {
            case true:
                for (float i = objpos.obj.transform.position.x; i < objpos.pos.x; i += 0.1f)
                {
                    objpos.obj.transform.position = new Vector3(i, objpos.obj.transform.position.y, objpos.obj.transform.position.z);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case false:
                for (float i = objpos.obj.transform.position.x; i > objpos.pos.x; i -= 0.1f)
                {
                    objpos.obj.transform.position = new Vector3(i, objpos.obj.transform.position.y, objpos.obj.transform.position.z);
                    yield return new WaitForSeconds(0.05f);
                }
                break;
        }
    }
}
