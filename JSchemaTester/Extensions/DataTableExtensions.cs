using System;
using System.Data;
using System.Linq;

namespace JSchemaTester.Extensions
{
    public static class DataTableExtensions
    {
        public static void PrintToConsole(this DataTable data)
        {
            string[] columnNames = data.Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .ToArray();

            Console.WriteLine(string.Join("|", columnNames.Select(c => c.PadRight(15, ' '))));
            Console.WriteLine(string.Empty.PadRight(15 * data.Columns.Count, '-'));

            foreach (DataRow row in data.Rows)
            {
                string[] fields = row.ItemArray
                    .Select(field => field.ToString())
                    .ToArray();

                Console.WriteLine(string.Join("|", fields.Select(c => c.PadRight(15, ' '))));
            }
        }
    }
}
