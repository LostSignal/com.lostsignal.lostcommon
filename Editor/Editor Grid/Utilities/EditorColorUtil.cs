//-----------------------------------------------------------------------
// <copyright file="EditorColorUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class EditorColorUtil
    {
        private static readonly Dictionary<Color, Texture2D> ColorCache = new Dictionary<Color, Texture2D>();

        public static bool IsProTheme()
        {
            return EditorGUIUtility.isProSkin;
        }

        public static Texture2D MakeColorTexture(Color col)
        {
            if (ColorCache.ContainsKey(col))
            {
                return ColorCache[col];
            }
            else
            {
                var pix = new Color32[1];
                pix[0] = col;

                Texture2D result = new Texture2D(1, 1)
                {
                    hideFlags = HideFlags.DontSave,
                };

                result.SetPixels32(pix);
                result.Apply();
                ColorCache[col] = result;
                return result;
            }
        }
    }
}
