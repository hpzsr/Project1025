using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager
{
    public static GameObject showLayer(Consts.Layer layerName)
    {
        return CommonUtil.createObjFromPrefab(GameObject.Find("Canvas").transform, "Prefabs/Layer/" + layerName.ToString());
    }

    public static void closeLayer(GameObject layer)
    {
        GameObject.Destroy(layer);
    }
}
