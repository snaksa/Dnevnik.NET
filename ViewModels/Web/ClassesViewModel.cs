﻿using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.ViewModels.Web
{
    public class ClassesViewModel
    {
        public Class ClassToAdd { get; set; }
        public List<Class> AllClasses { get; set; }
    }
}