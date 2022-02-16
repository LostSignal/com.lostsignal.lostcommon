//-----------------------------------------------------------------------
// <copyright file="ILevelLoadPostprocessor.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    public interface ILevelLoadPostprocessor
    {
        public bool IsProcessing { get; }
    }
}
