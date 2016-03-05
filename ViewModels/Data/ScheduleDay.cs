using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.ViewModels.Data
{
    public class ScheduleDay
    {
        public int Day { get; set; }
        public int Period { get; set; }
        public Subject Subject { get; set; }
    }
}