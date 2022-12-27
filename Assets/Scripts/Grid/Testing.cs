﻿using System.Collections.Generic;
using Pathfinder;
using UnitBased;
using UnityEngine;
using Utils;
using Utils.Camera;

namespace Grid
{
    public class Testing : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ScreenShake.Instance.Shake(5);
            }
        }
    }
}