using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public static class OnionAlignUtility
{
    public enum AlignHorizontal { Left, Center, Right, Average }
    public enum AlignVertical { Top, Center, Bottom, Average }

    public static void SetHorizontalAlign(this IEnumerable<Transform> elements, AlignHorizontal alignHorizontal)
    {
        float target;
        switch (alignHorizontal)
        {
            case AlignHorizontal.Left:

                target = elements.Select(_ => _.position.x).Min();

                foreach (var el in elements)
                    el.position = new Vector3(target, el.position.y, el.position.z);

                break;

            case AlignHorizontal.Center:

                target = (elements.Select(_ => _.position.x).Min() + elements.Select(_ => _.position.x).Max()) / 2F;

                foreach (var el in elements)
                    el.position = new Vector3(target, el.position.y, el.position.z);

                break;

            case AlignHorizontal.Right:

                target = elements.Select(_ => _.position.x).Max();

                foreach (var el in elements)
                    el.position = new Vector3(target, el.position.y, el.position.z);

                break;


            case AlignHorizontal.Average:

                if (elements.Count() <= 1)
                    throw new Exception("You have to select more than 1 object.");

                float min = elements.Select(_ => _.position.x).Min();
                float max = elements.Select(_ => _.position.x).Max();

                List<Transform> elList = elements.OrderBy(_ => _.position.x).ToList();
                for (int i = 0; i < elList.Count; i++)
                {
                    var item = elList[i];
                    item.position = new Vector3(Mathf.Lerp(min, max, (float)i / (elList.Count - 1)), item.position.y, item.position.z);
                }

                break;

        }
    }

    public static void SetVerticalAlign(this IEnumerable<Transform> elements, AlignVertical alignVertical)
    {
        float target;
        switch (alignVertical)
        {
            case AlignVertical.Top:

                target = elements.Select(_ => _.position.y).Max();

                foreach (var el in elements)
                    el.position = new Vector3(el.position.x, target, el.position.z);

                break;

            case AlignVertical.Center:

                target = (elements.Select(_ => _.position.y).Min() + elements.Select(_ => _.position.y).Max()) / 2F;

                foreach (var el in elements)
                    el.position = new Vector3(el.position.x, target, el.position.z);

                break;

            case AlignVertical.Bottom:

                target = elements.Select(_ => _.position.y).Min();

                foreach (var el in elements)
                    el.position = new Vector3(el.position.x, target, el.position.z);

                break;

            case AlignVertical.Average:

                if (elements.Count() <= 1)
                    throw new Exception("You have to select more than 1 object.");

                float min = elements.Select(_ => _.position.y).Min();
                float max = elements.Select(_ => _.position.y).Max();

                List<Transform> elList = elements.OrderBy(_ => _.position.y).ToList();
                for (int i = 0; i < elList.Count; i++)
                {
                    var item = elList[i];
                    item.position = new Vector3(item.position.x, Mathf.Lerp(min, max, (float)i / (elList.Count - 1)), item.position.z);
                }

                break;

        }
    }
}



