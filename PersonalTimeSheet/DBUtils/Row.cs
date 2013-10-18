using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalTimeSheet.DBUtils
{
    public class Row
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Enter the Description")]
        public string Description { get; set; }
        
        public TimeSpan SpentTime { get; set; }
    }
}