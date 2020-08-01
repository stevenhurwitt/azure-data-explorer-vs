using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.Data.Framework.AdoDotNet;

namespace AzureDataExplorer
{
    public class ADXConnectionProperties : AdoDotNetConnectionProperties
    {
        static readonly Dictionary<string, string[]> Synonyms;

        public override bool IsComplete => 
            !string.IsNullOrEmpty((string)this["Cluster"]) &&
            !string.IsNullOrEmpty((string)this["Database"]) &&
            (
                (bool)this["Integrated Security"] ||
                (!string.IsNullOrEmpty((string)this["Username"]) && !string.IsNullOrEmpty((string)this["Password"]))
            );

        public override string[] GetSynonyms(string key) => Synonyms[key];

        static ADXConnectionProperties()
        {
            Synonyms = typeof(KustoConnectionStringBuilder)
                .GetProperties()
                .Where(p => p.GetCustomAttribute<KustoConnectionStringPropertyAttribute>() != null)
                .ToDictionary(
                    p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                    p => p.GetCustomAttribute<KustoConnectionStringPropertyAttribute>().Synonyms
                );
        }
    }
}
