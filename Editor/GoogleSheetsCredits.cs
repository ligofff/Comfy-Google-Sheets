using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.ComfyGoogleSheets.Editor
{
    public class GoogleSheetsCredits : ScriptableObject
    {
        public List<GoogleSheetsTable> tables = new List<GoogleSheetsTable>();
        
        public IEnumerable<string> TableTitles => tables.Select(x => x.tableTitle);
        
        public string jsonKeyFilePath = "path";

        public GoogleSheetsTable GetTable(string tableTitle, bool initialize = true)
        {
            var table = tables.FirstOrDefault(x => x.tableTitle == tableTitle);
            if (table == null) return null;
            
            if (initialize) table.Initialize(GetService());
            
            return table;
        }
        
        public bool HasTable(string tableTitle) => tables.Any(x => x.tableTitle == tableTitle);

        public SheetsService GetService()
        {
            GoogleCredential credential = GoogleCredential.FromFile(jsonKeyFilePath)
                .CreateScoped(SheetsService.Scope.Spreadsheets);
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ComfyGoogleSheetsUnityPlugin"
            });

            return service;
        }
    }
}