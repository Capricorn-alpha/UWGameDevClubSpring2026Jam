using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public RectTransform map;
    private EnvironmentGroup group; 
    private Dictionary<Sprite, List<GameObject>> environmentDictionary;

    public void Start()
    {
        environmentDictionary = group.GetGroup(map);
    }
}
