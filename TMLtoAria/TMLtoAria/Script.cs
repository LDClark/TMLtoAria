using TMLtoAria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
    class Script
    {
        public Script()
        {
        }

        public void Execute(ScriptContext context)
        {
            Patient patient = context.Patient;
            if (patient == null)
                throw new ApplicationException("Please open a patient before using this script.");

            ExternalPlanSetup plan = context.ExternalPlanSetup;
            if (plan == null)
                throw new ApplicationException("Please select a plan before using this script.");

            String locPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String settingsFilePath = Path.Combine(locPath, @"TMLtoAria.setting");
            DocSettings docSettings = DocSettings.ReadSettings(settingsFilePath);

            var viewModel = new MainViewModel(context.CurrentUser, patient, plan, docSettings);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = viewModel;
            mainWindow.Title = "TMLtoAria";
            mainWindow.ShowDialog();
        }

        // Fix UnauthorizedScriptingAPIAccessException
        public void DoNothing(PlanSetup plan) { }
    }
}

