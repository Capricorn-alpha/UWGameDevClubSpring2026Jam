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

            if (images.Length == 2)
            {
                Sprite arrow = images[1].sprite;

                if (!environmentGroup.ContainsKey(arrow))
                {
                    environmentGroup[arrow] = new List<GameObject>();
                }

                environmentGroup[arrow].Add(environment.gameObject);
            }

            Sprite environmentSprite = images[0].sprite;

            if (!environmentGroup.ContainsKey(environmentSprite))
            {
                environmentGroup[environmentSprite] = new List<GameObject>();
            }

            environmentGroup[environmentSprite].Add(environment.gameObject);
        }

        return environmentGroup;
    }
}
