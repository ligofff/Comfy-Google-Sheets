using Plugins.ComfyGoogleSheets.Runtime;
using UnityEditor;
using UnityEngine;

namespace Plugins.ComfyGoogleSheets.Editor.SerializationModes
{
    public abstract class BaseTableRowSerializationMode
    {
        public bool CanSerializeToRow(Object asset)
        {
            return CanSerializeToRowInternal(asset);
        }

        protected abstract bool CanSerializeToRowInternal(Object asset);
        
        public TableRow SerializeToRow(Object asset)
        {
            return SerializeToRowInternal(asset);
        }
        
        protected abstract TableRow SerializeToRowInternal(Object asset);
        
        public void DeserializeFromRow(Object asset, TableRow row)
        {
            DeserializeFromRowInternal(asset, row);
            EditorUtility.SetDirty(asset);
        }

        protected abstract void DeserializeFromRowInternal(Object asset, TableRow row);
    }
}