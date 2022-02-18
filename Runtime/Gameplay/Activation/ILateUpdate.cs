//-----------------------------------------------------------------------
// <copyright file="ILateUpdate.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    public interface ILateUpdate
    {
        void OnLateUpdate(float deltaTime);
    }
}
