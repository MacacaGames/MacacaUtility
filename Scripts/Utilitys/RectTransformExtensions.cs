using UnityEngine;

namespace MacacaGames
{

    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll,

        UnKnown


    }

    public enum PivotPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    public static class RectTransformExtensions
    {
        public static AnchorPresets GetAnchorPresets(this RectTransform source)
        {
            AnchorPresets result = AnchorPresets.UnKnown;

            if (source.anchorMin == new Vector2(0, 1) &&
                source.anchorMax == new Vector2(0, 1))
            {
                result = AnchorPresets.TopLeft;
            }

            if (source.anchorMin == new Vector2(0.5f, 1) &&
                source.anchorMax == new Vector2(0.5f, 1))
            {
                result = AnchorPresets.TopCenter;
            }

            if (source.anchorMin == new Vector2(1, 1) &&
                source.anchorMax == new Vector2(1, 1))
            {
                result = AnchorPresets.TopRight;
            }

            if (source.anchorMin == new Vector2(0, 0.5f) &&
                source.anchorMax == new Vector2(0, 0.5f))
            {
                result = AnchorPresets.MiddleLeft;
            }
            if (source.anchorMin == new Vector2(0.5f, 0.5f) &&
                source.anchorMax == new Vector2(0.5f, 0.5f))
            {
                result = AnchorPresets.MiddleCenter;
            }

            if (source.anchorMin == new Vector2(1, 0.5f) &&
                source.anchorMax == new Vector2(1, 0.5f))
            {
                result = AnchorPresets.MiddleRight;
            }

            if (source.anchorMin == new Vector2(0, 0) &&
                source.anchorMax == new Vector2(0, 0))
            {
                result = AnchorPresets.BottomLeft;
            }

            if (source.anchorMin == new Vector2(0.5f, 0) &&
                source.anchorMax == new Vector2(0.5f, 0))
            {

                result = AnchorPresets.BottonCenter;
            }

            if (source.anchorMin == new Vector2(1, 0) &&
                source.anchorMax == new Vector2(1, 0))
            {
                result = AnchorPresets.BottomRight;
            }

            if (source.anchorMin == new Vector2(0, 1) &&
                source.anchorMax == new Vector2(1, 1))
            {
                result = AnchorPresets.HorStretchTop;
            }

            if (source.anchorMin == new Vector2(0, 0.5f) &&
                source.anchorMax == new Vector2(1, 0.5f))
            {
                result = AnchorPresets.HorStretchMiddle;
            }

            if (source.anchorMin == new Vector2(0, 0) &&
                source.anchorMax == new Vector2(1, 0))
            {
                result = AnchorPresets.HorStretchBottom;
            }

            if (source.anchorMin == new Vector2(0, 0) &&
                source.anchorMax == new Vector2(0, 1))
            {
                result = AnchorPresets.VertStretchLeft;
            }

            if (source.anchorMin == new Vector2(0.5f, 0) &&
                source.anchorMax == new Vector2(0.5f, 1))
            {
                result = AnchorPresets.VertStretchCenter;
            }

            if (source.anchorMin == new Vector2(1, 0) &&
                source.anchorMax == new Vector2(1, 1))
            {
                result = AnchorPresets.VertStretchRight;
            }

            if (source.anchorMin == new Vector2(0, 0) &&
                source.anchorMax == new Vector2(1, 1))
            {
                result = AnchorPresets.StretchAll;
            }

            return result;
        }


        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            source.anchoredPosition = new Vector3(source.anchoredPosition.x + offsetX, source.anchoredPosition.y + offsetY, 0);

            switch (allign)
            {
                case (AnchorPresets.TopLeft):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.TopCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 1);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.TopRight):
                    {
                        source.anchorMin = new Vector2(1, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.MiddleLeft):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(0, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0.5f);
                        source.anchorMax = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleRight):
                    {
                        source.anchorMin = new Vector2(1, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }

                case (AnchorPresets.BottomLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 0);
                        break;
                    }
                case (AnchorPresets.BottonCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 0);
                        break;
                    }
                case (AnchorPresets.BottomRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.HorStretchTop):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
                case (AnchorPresets.HorStretchMiddle):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }
                case (AnchorPresets.HorStretchBottom):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.VertStretchLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.StretchAll):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
            }
        }

        public static void SetPivot(this RectTransform source, PivotPresets preset)
        {

            switch (preset)
            {
                case (PivotPresets.TopLeft):
                    {
                        source.pivot = new Vector2(0, 1);
                        break;
                    }
                case (PivotPresets.TopCenter):
                    {
                        source.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (PivotPresets.TopRight):
                    {
                        source.pivot = new Vector2(1, 1);
                        break;
                    }

                case (PivotPresets.MiddleLeft):
                    {
                        source.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleRight):
                    {
                        source.pivot = new Vector2(1, 0.5f);
                        break;
                    }

                case (PivotPresets.BottomLeft):
                    {
                        source.pivot = new Vector2(0, 0);
                        break;
                    }
                case (PivotPresets.BottomCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (PivotPresets.BottomRight):
                    {
                        source.pivot = new Vector2(1, 0);
                        break;
                    }
            }
        }
    }
}