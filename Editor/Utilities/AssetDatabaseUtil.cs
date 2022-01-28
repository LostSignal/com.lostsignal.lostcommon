//-----------------------------------------------------------------------
// <copyright file="AssetDatabaseUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class AssetDatabaseUtil
    {
        public static IEnumerable<GameObject> GetAllProjectPrefabs()
        {
            return AssetDatabase.GetAllAssetPaths()
                .Where(x => x.ToLower().EndsWith(".prefab"))
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));
        }
    }
}
