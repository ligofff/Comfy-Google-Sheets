using System;
using System.Linq;
using System.Reflection;
using Plugins.ComfyGoogleSheets.Runtime;
using Sirenix.Utilities;
using Object = UnityEngine.Object;

namespace Plugins.ComfyGoogleSheets.Editor.SerializationModes.Implemented
{
    [Serializable]
    public class AttributeSerializationMode : BaseTableRowSerializationMode
    {
        protected override bool CanSerializeToRowInternal(Object asset)
        {
            return asset
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(field => field.GetAttribute<TableRowValueAttribute>() is { });
        }

        protected override TableRow SerializeToRowInternal(Object asset)
        {
            var fields = asset.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(field => field.GetAttribute<TableRowValueAttribute>() is { });
            
            var row = new TableRow(
                $"{asset.name}_{asset.GetType().Name}",
                fields.Select(field => new TableRow.ValuePair(field.Name, field.GetValue(asset).ToString())));

            return row;
        }

        protected override void DeserializeFromRowInternal(Object asset, TableRow row)
        {
            var fields = asset.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(field => field.GetAttribute<TableRowValueAttribute>() is { })
                .ToDictionary(field => field.Name, field => field);

            foreach (var valuePair in row.Values)
            {
                if (fields.TryGetValue(valuePair.columnName, out var field))
                {
                    field.SetValue(asset, valuePair.value);
                }
            }
        }
    }
}