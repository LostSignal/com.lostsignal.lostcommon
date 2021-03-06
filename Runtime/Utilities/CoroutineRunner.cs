//-----------------------------------------------------------------------
// <copyright file="CoroutineRunner.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class CoroutineRunner : SingletonMonoBehaviour<CoroutineRunner>, IName
    {
        string IName.Name => "Coroutine Runner";
    }
}

#endif
