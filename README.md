# [**WIP**] Comfy Google Sheets
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

## About
Comfy Google Sheets is a Unity package that facilitates easy pushing and pulling engine assets data to/from Google Sheets.<br />

> :warning: **Currently asset in WIP stage! Can contains bugs and dirty, unoptimized code!**

> [My Telegram channel](https://t.me/ligofff_blog) if you want more

## Overview
Comfy Google Sheets simplifies the process by providing an easy-to-use interface that integrates directly with Google Sheets.<br />

Make a simple row converter, select multuple scriptable objects, and push it into google sheet!
Then, make your mass assets changes, using table formulas and other things, and pull all data back into your engine assets!<br />

This tool perfectly fits into your workflow and improves the speed and efficiency of your game development, making it even comfier!

Little Demo:<br />

https://github.com/fffogil/Comfy-Google-Sheets/assets/44195161/7c23d294-8e91-4bee-a333-6f2cfb5878ea


## Minimum Requirements
* The tool is designed for Unity 2022.2 and higher versions.
* [Odin Inspector](https://odininspector.com/)
* [NuGet Google APIs Client Library for working with Sheets v4](https://www.nuget.org/packages/Google.Apis.Sheets.v4/) *(Be sure to install with dependencies - Apis, Apis.Core, Apis.Auth)*

### Install via GIT URL
Go to ```Package Manager``` -> ```Add package from GIT url...``` -> Enter ```https://github.com/ligofff/Comfy-Google-Sheets.git``` -> Click ```Add```

You will need to have Git installed and available in your system's PATH.

## Usage

After installing, you will see the ```ComfyGoogleSheets``` folder in your ```Packages``` folder.

To use Comfy Google Sheets:

1. Do not forget to install [Google APIs Library](https://www.nuget.org/packages/Google.Apis.Sheets.v4/) into your unity project

2. **Create service account** in your Google API panel, **download json credits** and **share table with service account**. Its simple! More explanation in [this YouTube video](https://youtu.be/qm-Ooj6XjvE?si=XrFPrs7yXQgyMrKT)

3. Create GoogleSheetsCredits asset, and fill it
<p align="center">
  <img width="500" src="https://github.com/fffogil/Comfy-Google-Sheets/assets/44195161/3d6e8339-31d2-4e76-b68d-bea0c6555b5d">
</p>

4. Describe your assets serializing/deserializing behaviour in one of these ways:
     * Mark fields with ```TableRowValue``` attribute
     * Implement ```ITableRowConverter``` interface in your asset classes
     * Or create any additional class anywhere in your project, and implement ```IExternalAssetTableRowConverter``` interface for it. *(This can be used for serialize classes that you cannot edit)*
  
5. Create ```ComfyGoogleSheetsAssetsContainer``` scriptable object anywhere in project, select prepared credits, table, assets, and continue make your game!
<p align="center">
  <img width="500" src="https://github.com/fffogil/Comfy-Google-Sheets/assets/44195161/ba32646f-42ba-4fb3-b4c5-f1424926e405">
</p>

**That's all!** Start manipulating your engine assets data in a more comfy way.

## Code sample

### Its my ```ITableRowConverter``` interface implementation for creature statistics data asset

```csharp
        // Collect stats and make TableRow object from it
        public TableRow SerializeToTableRow()
        {
            return new TableRow(name, stats.Select(stat => new TableRow.ValuePair(stat.GetType().Name, stat.BaseValue.ToString())));
        }

        // Take TableRow object, read it, and set stats values
        public void DeserializeFromTableRow(TableRow row)
        {
            foreach (var valuePair in row.Values)
            {
                var statName = valuePair.columnName;
                var stat = stats.FirstOrDefault(x => x.GetType().Name == statName);
                if (stat == null) continue;
                
                stat.GetType().GetField("baseValue", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(stat, float.Parse(valuePair.value));
            }
        }
```

## License

MIT License

Copyright (c) 2022 Ligofff

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
