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

    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        public static CoroutineRunner Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (instance == null)
                {
                    instance = SingletonUtil.CreateSingleton<CoroutineRunner>("Coroutine Runner");
                }

                return instance;
            }
        }
    }
}

#endif
