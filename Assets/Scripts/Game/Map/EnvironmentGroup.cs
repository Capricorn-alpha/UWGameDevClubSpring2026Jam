using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnvironmentGroup : MonoBehaviour
{
    private Dictionary<Sprite, List<GameObject>> environmentGroup = new Dictionary<Sprite, List<GameObject>>();

    public Dictionary<Sprite, List<GameObject>> GetGroup(RectTransform map)
    {
        environmentGroup.Clear();

        foreach(RectTransform environment in map)
        {
            Image[] images = environment.GetComponentsInChildren<Image>();

            if (images == null || images.Length == 0)
                continue;

            foreach (Image image in images)
            {
                if (image.sprite == null)
                    continue;

                Sprite sprite = image.sprite;

                if (!environmentGroup.ContainsKey(sprite))
                {
                    environmentGroup[sprite] = new List<GameObject>();
                }

                environmentGroup[sprite].Add(environment.gameObject);
            }
        }

        return environmentGroup;
    }
}
