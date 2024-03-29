﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MacacaGames
{
    public static class FloatExtension
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static int FloorToInt (this float value)
        {
            var valueDecimal = new decimal(value);
            return (int)decimal.Floor(valueDecimal);
        }
    }
}
