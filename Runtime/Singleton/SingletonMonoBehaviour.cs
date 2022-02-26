//-----------------------------------------------------------------------
// <copyright file="SingletonMonoBehaviour.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour, IName
    {
        private static T instance;
        private static bool initialized;

        public static T Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => initialized ? instance : Initialize();
        }

        private static T Initialize()
        {
            instance = SingletonUtil.CreateSingleton<T>(string.Empty);
            instance.gameObject.name = instance.Name;
            initialized = true;

            return instance;
        }
    }
}
