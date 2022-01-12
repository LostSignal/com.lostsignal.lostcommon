//-----------------------------------------------------------------------
// <copyright file="PackageMapperRepository.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class PackageMapperRepository
    {
        #pragma warning disable 0649
        [SerializeField] private string packageIdentifier;
        [SerializeField] private string gitHubUrl;
        [SerializeField] private List<string> localSourceDirectories;
        #pragma warning restore 0649

        public string PackageIdentifier
        {
            get => this.packageIdentifier;
            set => this.packageIdentifier = value;
        }

        public string GitHubUrl
        {
            get => this.gitHubUrl;
            set => this.gitHubUrl = value;
        }

        public List<string> LocalSourceDirectories
        {
            get => this.localSourceDirectories;
            set => this.localSourceDirectories = value;
        }
    }
}
