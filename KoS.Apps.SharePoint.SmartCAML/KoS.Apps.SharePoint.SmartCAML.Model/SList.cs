﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoS.Apps.SharePoint.SmartCAML.Model
{
    public class SList
    {
        public Web Web { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsHidden { get; set; }

        public List<Field> Fields { get; set; } = new List<Field>();

    }
}