using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugins.ComfyGoogleSheets.Runtime
{
    [Serializable]
    public class TableRow
    {
        [Serializable]
        public class ValuePair
        {
            public string columnName;
            public string value;

            public ValuePair(string columnName, string value)
            {
                this.columnName = columnName;
                this.value = value;
            }
        }
        
        public string Key => key;
        public IReadOnlyList<ValuePair> Values => values;

        [SerializeField]
        private List<ValuePair> values;

        [SerializeField]
        private string key;

        public TableRow(string key, IEnumerable<ValuePair> values)
        {
            this.key = key;
            this.values = values.ToList();
        }
    }
}