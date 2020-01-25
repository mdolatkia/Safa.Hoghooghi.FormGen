using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelGenerator
{
   public class ModelGenerator
    {
        public static I_DatabaseImportHelper GetDatabaseImportHelper(DatabaseDTO database)
        {
            return new ModelGenerator_sql(database);
        }
    }
}
