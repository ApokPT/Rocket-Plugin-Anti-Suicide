using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ApokPT.RocketPlugins
{
    class SpawnLocation
    {
        public Vector3 Position { get; private set; }
        public float Angle { get; private set; }

        public SpawnLocation(Vector3 position, float angle)
        {
            Position = position;
            Angle = angle;
        }
    }
}
