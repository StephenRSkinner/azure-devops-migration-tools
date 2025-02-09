﻿using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using VstsSyncMigrator.Engine.Configuration.Processing;

namespace VstsSyncMigrator.Engine
{
    public class WorkItemUpdate : ProcessingContextBase
    {
        WorkItemUpdateConfig _config;
        MigrationEngine _me;

        public WorkItemUpdate(MigrationEngine me, WorkItemUpdateConfig config) : base(me, config)
        {
            _me = me;
            _config = config;
        }

        public override string Name
        {
            get
            {
                return "WorkItemUpdate";
            }
        }

        internal override void InternalExecute()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
			//////////////////////////////////////////////////
			WorkItemStoreContext targetStore = new WorkItemStoreContext(me.Target, WorkItemStoreFlags.BypassRules);

            TfsQueryContext tfsqc = new TfsQueryContext(targetStore);
            tfsqc.AddParameter("TeamProject", me.Target.Config.Name);
            tfsqc.Query = string.Format(@"SELECT [System.Id], [System.Tags] FROM WorkItems WHERE [System.TeamProject] = @TeamProject {0} ORDER BY [System.ChangedDate] desc", _config.QueryBit);
            WorkItemCollection  workitems = tfsqc.Execute();
            Trace.WriteLine(string.Format("Update {0} work items?", workitems.Count));
            //////////////////////////////////////////////////
            int current = workitems.Count;
            int count = 0;
            long elapsedms = 0;
            foreach (WorkItem workitem in workitems)
            {
                Stopwatch witstopwatch = Stopwatch.StartNew();
				workitem.Open();
                Trace.WriteLine(string.Format("Processing work item {0} - Type:{1} - ChangedDate:{2} - CreatedDate:{3}", workitem.Id, workitem.Type.Name, workitem.ChangedDate.ToShortDateString(), workitem.CreatedDate.ToShortDateString()));
                _me.ApplyFieldMappings(workitem);

                if (workitem.IsDirty)
                {
                    if (!_config.WhatIf)
                    {
                        try
                        {
                            workitem.Save();
                        }
                        catch (Exception)
                        {
                            System.Threading.Thread.Sleep(5000);
                            workitem.Save();
                        }
                       
                    } else
                    {
                        Trace.WriteLine("No save done: (What IF: enabled)");
                    }
                    
                } else
                {
                    Trace.WriteLine("No save done: (IsDirty: false)");
                }
                



                witstopwatch.Stop();
                elapsedms = elapsedms + witstopwatch.ElapsedMilliseconds;
                current--;
                count++;
                TimeSpan average = new TimeSpan(0, 0, 0, 0, (int)(elapsedms / count));
                TimeSpan remaining = new TimeSpan(0, 0, 0, 0, (int)(average.TotalMilliseconds * current));
                Trace.WriteLine(string.Format("Average time of {0} per work item and {1} estimated to completion", string.Format(@"{0:s\:fff} seconds", average), string.Format(@"{0:%h} hours {0:%m} minutes {0:s\:fff} seconds", remaining)));
            }
            //////////////////////////////////////////////////
            stopwatch.Stop();
            Console.WriteLine(@"DONE in {0:%h} hours {0:%m} minutes {0:s\:fff} seconds", stopwatch.Elapsed);
        }

    }
}