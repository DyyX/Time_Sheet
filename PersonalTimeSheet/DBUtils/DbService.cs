using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalTimeSheet.DBUtils
{
    public class DbService
    {
        #region Privats

        private List<Row> _rows = new List<Row>();

        private void FillRowsList()
        {
            if (0 != _rows.Count)
            {
                _rows.Clear();
            }

            using (var context = new PTSDataBaseEntities())
            {
                foreach (var row in context.Table)
                {
                    _rows.Add(DbTableToLocalTable(row));
                }
            }
        }

        #endregion

        #region Publics

        public Row DbTableToLocalTable(Table table)
        {
            return new Row()
                {
                    Description = table.DbDescription,
                    Id = table.DbId,
                    SpentTime = TimeSpan.FromSeconds(table.DbSpentTime ?? 0),
                    Title = table.DbTitle
                };
        }



        public List<Row> Rows
        {
            get
            {
                if (0 == _rows.Count)
                {
                    FillRowsList();
                }

                return _rows;
            }

            private set { }
        }

        public void AddTableRow(Row row)
        {
            using (var context = new PTSDataBaseEntities())
            {
                context.Table.Add(new Table()
                    {
                        DbDescription = row.Description,
                        DbSpentTime = (long) row.SpentTime.TotalSeconds,
                        DbTitle = row.Title
                    });

                context.SaveChanges();
               _rows.Add(row);
            }
        }

        public void UpdateTableRow(Row row)
        {
            using (var context = new PTSDataBaseEntities())
            {
                var dbRow = context.Table.SingleOrDefault(r => r.DbId == row.Id);
                if (null != dbRow)
                {
                    dbRow.DbDescription = row.Description;
                    dbRow.DbSpentTime = (long) row.SpentTime.TotalSeconds;
                    dbRow.DbTitle = row.Title;

                    context.SaveChanges();
                    FillRowsList();
                }
            }
        }

        public void RemoveTableRow(int rowId)
        {
            using (var context = new PTSDataBaseEntities())
            {
                var dbRow = context.Table.SingleOrDefault(r => r.DbId == rowId);
                if (null != dbRow)
                {
                    context.Table.Remove(dbRow);

                    context.SaveChanges();
                    _rows.Remove(_rows.SingleOrDefault(r => r.Id == rowId));
                }

            }
        }

        #endregion
    }
}