using System;
using Object = UnityEngine.Object;

namespace Plugins.ComfyGoogleSheets.Runtime
{
    public interface IExternalAssetTableRowConverter
    {
        public Type GetSerializationType { get; }

        public TableRow SerializeToTableRow(Object asset);

        public void DeserializeFromTableRow(TableRow row, Object asset);
    }
}