﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VstsSyncMigrator.Engine.Configuration
{
    public class TeamProjectConfig
    {
        public Uri Collection { get; set; }
        public string Name { get; set; }
        public string ReflectedWorkItemIDFieldName { get; set; }
    }
}
