using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugins.ComfyGoogleSheets.Runtime;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.ComfyGoogleSheets.Editor
{
    [Serializable]
    public class GoogleSheetsTable
    {
        public string tableTitle;

        public string spreadsheetId;

        public string tabName = "Sheet1";
        
        public int maxRows = 300;
        
        public GoogleSheetsTable(string tableTitle)
        {
            this.tableTitle = tableTitle;
        }
        
        private SheetsService service;
        
        private string[,] _tempTable;

        public void Initialize(SheetsService service)
        {
            this.service = service;
            _tempTable = new string[26, maxRows];
            
            var columnKeys = GetColumnKeys();

            for (int i = 0; i < columnKeys.Count; i++)
            {
                var columnKey = columnKeys[i];
                _tempTable[i, 0] = columnKey;
            }

            var rowKeys = GetRowKeys();
            
            for (int i = 1; i < rowKeys.Count; i++)
            {
                var rowKey = rowKeys[i];
                _tempTable[0, i+1] = rowKey;
            }
            
            _tempTable[0, 0] = "---";
        }

        public void WriteRow(TableRow row)
        {
            // Find row index
            int rowIndex = -1;
            int latestRowIndexWithValue = -1;
            
            for (int i = 0; i < _tempTable.GetLength(1); i++)
            {
                var value = _tempTable[0, i];
                
                if (!(string.IsNullOrEmpty(value) || value == "-")) latestRowIndexWithValue = i;
                
                if (value == row.Key)
                {
                    rowIndex = i;
                    break;
                }
            }

            if (rowIndex == -1)
                rowIndex = latestRowIndexWithValue + 1;

            // Set row key
            _tempTable[0, rowIndex] = row.Key;
            
            // Set row values
            foreach (var valuePair in row.Values)
            {
                // Find column index
                int columnIndex = -1;
                int latestColumnIndexWithValue = -1;
            
                for (int i = 0; i < _tempTable.GetLength(0); i++)
                {
                    var value = _tempTable[i, 0];
                    if (!(string.IsNullOrEmpty(value) || value == "-")) latestColumnIndexWithValue = i;

                    if (value == valuePair.columnName)
                    {
                        columnIndex = i;
                        break;
                    }
                }

                if (columnIndex == -1)
                    columnIndex = latestColumnIndexWithValue + 1;
                
                // Set column name and value
                _tempTable[columnIndex, 0] = valuePair.columnName;
                _tempTable[columnIndex, rowIndex] = valuePair.value;
            }
        }

        public void PushTable()
        {
            // Convert _tempTable to List<IList<object>>()
            List<IList<object>> list = new List<IList<object>>();
            for (int j = 0; j < _tempTable.GetLength(1); j++)
            {
                IList<object> column = new List<object>();
                for (int i = 0; i < _tempTable.GetLength(0); i++)
                {
                    if (string.IsNullOrEmpty(_tempTable[i, j]))
                        _tempTable[i, j] = "-";
                    column.Add(_tempTable[i, j]);
                }
                list.Add(column);
            }

            var valueRange = new ValueRange();
            valueRange.Values = list;

            SpreadsheetsResource.ValuesResource.UpdateRequest request =
                service.Spreadsheets.Values.Update(valueRange, spreadsheetId, $"{tabName}!A1");
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                
            UpdateValuesResponse response = request.Execute();
        }
        
        public void PullTable()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, $"{tabName}!A1:Z{maxRows}");
            
            ValueRange response = request.Execute();

            // Add all response values to _tempTable
            for (int i = 0; i < response.Values.Count; i++)
            {
                for (int j = 0; j < response.Values[i].Count; j++)
                {
                    var value = (string)response.Values[i][j];
                    _tempTable[j, i] = value;
                }
            }
        }

        public TableRow ReadRow(string key)
        {
            var rowIndex = -1;
            
            for (int i = 0; i < _tempTable.GetLength(1); i++)
            {
                if (_tempTable[0, i] == key)
                {
                    rowIndex = i;
                    break;
                }
            }
            
            if (rowIndex == -1) throw new Exception($"Row with given key not found! {key}");

            var values = new List<TableRow.ValuePair>();
            
            for (int i = 0; i < _tempTable.GetLength(0); i++)
            {
                values.Add(new TableRow.ValuePair(_tempTable[i, 0], _tempTable[i, rowIndex]));
            }
            
            return new TableRow(_tempTable[0, rowIndex], values);
        }

        public List<string> GetRowKeys()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, $"{tabName}!A2:A{maxRows}");
            
            ValueRange response = request.Execute();

            return response.Values == null ? new List<string>() : response.Values.SelectMany(rsp => rsp).OfType<string>().ToList();
        }
        
        public List<string> GetColumnKeys()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, $"{tabName}!A1:Z1");
            
            ValueRange response = request.Execute();

            return response.Values == null ? new List<string>() : response.Values.SelectMany(rsp => rsp).OfType<string>().ToList();
        }

        public void ClearTable()
        {
            _tempTable = new string[26, maxRows];
            
            PushTable();
        }
    }
}