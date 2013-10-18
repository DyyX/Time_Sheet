using System.Collections.Generic;
using PersonalTimeSheet.DBUtils;

namespace PersonalTimeSheet.Models
{
    public class RowListViewModel
    {
        public List<Row> Rows;
        public PagingInfo Info;
    }
}