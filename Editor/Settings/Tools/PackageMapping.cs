//-----------------------------------------------------------------------
// <copyright file="PackageMapping.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PackageMapping
    {
        #pragma warning disable 0649
        [SerializeField] private string packageIdentifier;
        [SerializeField] private string gitHubUrl;
        [SerializeField] private string localPath;
        #pragma warning restore 0649

        public string PackageIdentifier
        {
            get => this.packageIdentifier;
            set => this.packageIdentifier = value;
        }

        public string GitUrl
        {
            get => this.gitHubUrl;
            set => this.gitHubUrl = value;
        }

        public string LocalPath
        {
            get => this.localPath;

            set
            {
                if (value != null)
                {
                    this.localPath = value.Replace("\\", "/");
                }
                else
                {
                    this.localPath = value;
                }
            }
        }
    }
}
