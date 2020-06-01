using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject_WPF
{
    public class WizardHelper
    {
        public static Tuple<string, string> GetTablesAliasAndDescriptions(List<TableDrivedEntityDTO> entities)
        {
            string entitytitleTag = ""; string entitydescTag = "";
            if (entities.Any(x => x.DatabaseDescriptions.Any()))
            {
                var allDescrioptions = entities.SelectMany(x => x.DatabaseDescriptions);

                if (allDescrioptions.Count(y => y.Item1.ToLower().Contains("title")) > entities.Count / 2)
                    entitytitleTag = allDescrioptions.Where(y => y.Item1.ToLower().Contains("title")).GroupBy(x => x.Item1).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Item1.ToLower().Contains("name")) > entities.Count / 2)
                    entitytitleTag = allDescrioptions.Where(y => y.Item1.ToLower().Contains("name")).GroupBy(x => x.Item1).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Item1.ToLower().Contains("alias")) > entities.Count / 2)
                    entitytitleTag = allDescrioptions.Where(y => y.Item1.ToLower().Contains("alias")).GroupBy(x => x.Item1).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                if (string.IsNullOrEmpty(entitytitleTag))
                {
                    entitytitleTag = allDescrioptions.Where(x => !x.Item2.Contains(" ") && x.Item2.Length > 5 && x.Item2.Length <= 20).GroupBy(x => x.Item1).OrderByDescending(y => y.Count()).FirstOrDefault()?.Key.ToLower();
                }


                if (allDescrioptions.Count(y => y.Item1.ToLower().Contains("description")) > entities.Count / 2)
                    entitydescTag = allDescrioptions.Where(y => y.Item1.ToLower().Contains("description")).GroupBy(x => x.Item1).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Item1.ToLower().Contains("comment")) > entities.Count / 2)
                    entitydescTag = allDescrioptions.Where(y => y.Item1.ToLower().Contains("comment")).GroupBy(x => x.Item1).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                if (string.IsNullOrEmpty(entitydescTag))
                {
                    entitydescTag = allDescrioptions.Where(x => x.Item2.Contains(" ") && x.Item2.Length > 20).GroupBy(x => x.Item1).OrderByDescending(y => y.Count()).FirstOrDefault()?.Key.ToLower();
                }

            }



            return new Tuple<string, string>(entitytitleTag, entitydescTag);

        }
        public static Tuple<string, string> GetColumnsAliasAndDescriptions(List<ColumnDTO> columns)
        {
            string columntitleTag = ""; string columndescTag = "";


            if (columns.Any(x => x.DatabaseDescriptions.Any()))
            {
                var allDescrioptions = columns.SelectMany(x => x.DatabaseDescriptions);

                if (allDescrioptions.Count(y => y.Key.ToLower().Contains("title")) > columns.Count / 2)
                    columntitleTag = allDescrioptions.Where(y => y.Key.ToLower().Contains("title")).GroupBy(x => x.Key).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Key.ToLower().Contains("name")) > columns.Count / 2)
                    columntitleTag = allDescrioptions.Where(y => y.Key.ToLower().Contains("name")).GroupBy(x => x.Key).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Key.ToLower().Contains("alias")) > columns.Count / 2)
                    columntitleTag = allDescrioptions.Where(y => y.Key.ToLower().Contains("alias")).GroupBy(x => x.Key).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                if (string.IsNullOrEmpty(columntitleTag))
                {
                    columntitleTag = allDescrioptions.Where(x => !x.Value.Contains(" ") && x.Value.Length > 5 && x.Value.Length <= 20).GroupBy(x => x.Key).OrderByDescending(y => y.Count()).FirstOrDefault()?.Key.ToLower();
                }


                if (allDescrioptions.Count(y => y.Key.ToLower().Contains("description")) > columns.Count / 2)
                    columndescTag = allDescrioptions.Where(y => y.Key.ToLower().Contains("description")).GroupBy(x => x.Key).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                else if (allDescrioptions.Count(y => y.Key.ToLower().Contains("comment")) > columns.Count / 2)
                    columndescTag = allDescrioptions.Where(y => y.Key.ToLower().Contains("comment")).GroupBy(x => x.Key).OrderByDescending(x => x.Count()).FirstOrDefault()?.Key.ToLower();
                if (string.IsNullOrEmpty(columndescTag))
                {
                    columndescTag = allDescrioptions.Where(x => x.Value.Contains(" ") && x.Value.Length > 20).GroupBy(x => x.Key).OrderByDescending(y => y.Count()).FirstOrDefault()?.Key.ToLower();
                }

            }
            return new Tuple<  string, string>(  columntitleTag, columndescTag);

        }
        public static void SetEntityAliasAndDescription(TableImportItem item, string entitytitleTag, string entitydescTag)
        {
            if (!string.IsNullOrEmpty(entitytitleTag) || !string.IsNullOrEmpty(entitydescTag))
            {

                if (!string.IsNullOrEmpty(entitytitleTag))
                {
                    if (item.Entity.DatabaseDescriptions.Any(x => x.Item1.ToLower() == entitytitleTag))
                    {
                        item.Entity.Alias = item.Entity.DatabaseDescriptions.First(x => x.Item1.ToLower() == entitytitleTag).Item2;
                    }
                }
                if (!string.IsNullOrEmpty(entitydescTag))
                {
                    if (item.Entity.DatabaseDescriptions.Any(x => x.Item1.ToLower() == entitydescTag))
                    {
                        item.Entity.Description = item.Entity.DatabaseDescriptions.First(x => x.Item1.ToLower() == entitydescTag).Item2;
                    }
                }

            }
        }
        public static void SetColumnAliasAnadDescription(ColumnDTO column, string columntitleTag, string columndescTag)
        {
            if (!string.IsNullOrEmpty(columntitleTag) || !string.IsNullOrEmpty(columndescTag))
            {
                if (!string.IsNullOrEmpty(columntitleTag))
                {
                    if (column.DatabaseDescriptions.Any(x => x.Key.ToLower() == columntitleTag))
                    {
                        column.Alias = column.DatabaseDescriptions.First(x => x.Key.ToLower() == columntitleTag).Value;
                    }
                }
                if (!string.IsNullOrEmpty(columndescTag))
                {
                    if (column.DatabaseDescriptions.Any(x => x.Key.ToLower() == columndescTag))
                    {
                        column.Description = column.DatabaseDescriptions.First(x => x.Key.ToLower() == columndescTag).Value;
                    }
                }
            }
        }
    }
}
