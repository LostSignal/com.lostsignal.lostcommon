//-----------------------------------------------------------------------
// <copyright file="Area.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using UnityEngine;

    public enum AreaType
    {
        Sphere,
        Cylinder,
        Box,
    }

    [Serializable]
    public struct Area
    {
        public AreaType Type;
        public Vector3 Size;

        public bool IsInside(Matrix4x4 worldToLocalMatrix, Vector3 worldPosition)
        {
            var localPostiion = worldToLocalMatrix.MultiplyPoint(worldPosition);

            if (this.Type == AreaType.Sphere)
            {
                return localPostiion.sqrMagnitude < (this.Size.x * this.Size.x);
            }
            else if (this.Type == AreaType.Cylinder)
            {
                return localPostiion.sqrMagnitude < (this.Size.x * this.Size.x) && 
                       localPostiion.y < this.Size.y / 2.0f &&
                       localPostiion.y > -this.Size.y / 2.0f;
            }
            else if (this.Type == AreaType.Box)
            {
                return localPostiion.x < this.Size.x / 2.0f &&
                       localPostiion.x > -this.Size.x / 2.0f &&
                       localPostiion.y < this.Size.y / 2.0f &&
                       localPostiion.y > -this.Size.y / 2.0f &&
                       localPostiion.z < this.Size.z / 2.0f &&
                       localPostiion.z > -this.Size.z / 2.0f;
            }
            else
            { 
                throw new NotImplementedException($"{nameof(Area)} IsInside encountered unknown {nameof(AreaType)} {this.Type}");
            }
        }
    }
}
