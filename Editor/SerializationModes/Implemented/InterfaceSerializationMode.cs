using System;
using Plugins.ComfyGoogleSheets.Runtime;
using Object = UnityEngine.Object;

namespace Plugins.ComfyGoogleSheets.Editor.SerializationModes.Implemented
{
    [Serializable]
    public class InterfaceSerializationMode : BaseTableRowSerializationMode
    {
        protected override bool CanSerializeToRowInternal(Object asset)
        {
            return asset is ITableRowConverter;
        }

        protected override TableRow SerializeToRowInternal(Object asset)
        {
            return ((ITableRowConverter)asset).SerializeToTableRow();
        }

        protected override void DeserializeFromRowInternal(Object asset, TableRow row)
        {
            ((ITableRowConverter)asset).DeserializeFromTableRow(row);
        }
    }
}