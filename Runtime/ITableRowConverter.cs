namespace Plugins.ComfyGoogleSheets.Runtime
{
    public interface ITableRowConverter
    {
        public TableRow SerializeToTableRow();

        public void DeserializeFromTableRow(TableRow row);
    }
}