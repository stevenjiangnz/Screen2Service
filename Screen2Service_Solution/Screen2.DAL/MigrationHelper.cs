using Screen2.Entity;
using Screen2.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.DAL
{
    public class MigrationHelper
    {

        public void SeedHelper(DataContext context)
        {
            // Insert BusinessUnit lookup
            InsertMarket(context);
            InsertSetting(context);
            //InsertSetting(context);
        }

        private void InsertMarket(DataContext context)
        {
            context.Markets.AddOrUpdate(
                p => p.Name,
                new Market { Name = "ASX", Description = "Australia Exchange", SymbolSuffix = "ax" },
                new Market { Name = "NZX", Description = "New Zealand Exchange", SymbolSuffix = "nz" }
                );
        }

        private void InsertSetting(DataContext context)
        {
            context.Settings.AddOrUpdate(
                p => p.Key,
                new Setting { Key = SettingKey.HistoryDataYears.ToString(), Value="20" },
                new Setting { Key = SettingKey.YahooProfileUrl.ToString(), Value = "https://au.finance.yahoo.com/q/pr?s={0}" }
                );
        }
    }
}
