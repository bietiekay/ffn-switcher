using System;
using System.Collections.Generic;
using System.Text;
using FFN_Switcher.Logging;
using FFN_Switcher.Settings;
using System.IO;

namespace FFN_Switcher.HTTP
{
    /// <summary>
    /// each file request that leads to a .html file is passed through here
    /// </summary>
    public class TemplateProcessor
    {
        #region internal Data
        private Settings.Settings internalSettings;
        private List<String> SettingsNames;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the Templace Processor
        /// </summary>
        /// <param name="vcrscheduler">the VCRScheduler...so we can get the </param>
        public TemplateProcessor(Settings.Settings Settings)
        {
            internalSettings = Settings;

            SettingsNames = internalSettings.GetSettingsNameListings();
        }
        #endregion

        #region ProcessHTMLTemplate
        /// <summary>
        /// Processes the actual HTML Sourcecode
        /// </summary>
        /// <param name="template_filename"></param>
        /// <returns>the actual HTML code after the template parsing and keyword replaceing</returns>
        public String ProcessHTMLTemplate(String template_filename, String Querystring)
        {
            String Output_HTML_Code = "";
            DateTime Started = DateTime.Now;

            #region read the template file
            using (StreamReader sr = File.OpenText(template_filename))
            {
                String buffer;

                while ((buffer = sr.ReadLine()) != null)
                {
                    Output_HTML_Code = Output_HTML_Code + buffer;
                    // read...
                }
                sr.Close();
            }
            #endregion

            #region replace the placeholders...

            // TODO: at the moment the placeholders must be lower case only to be detected - change that!
            // TODO: 3 times the same keyword... change that
            #region no need for querystring or optional

            #region %include($templateURL)%
            // first find the placeholder

            // detect if we included a file again in the last 10 iterations
            List<string> LoopDetection = new List<string>();


            while (Output_HTML_Code.Contains("%include("))
            {

                // TODO: make a URL version to include http URLs as well
                // now this is the first placeholder that has a parameter, the next step would be to extract the parameter...
                try
                {
                    #region Find and extract the parameter, then delete it with %include( in front of and ) behind
                    int StartPosition = Output_HTML_Code.IndexOf("%include(");

                    // add the parameter...
                    StartPosition = StartPosition + 9;

                    // we need a working copy...it's easier...
                    String parameters = Output_HTML_Code.Remove(0, StartPosition);

                    // let's find the next ) and remove everything, including the ) afterwards
                    StartPosition = parameters.IndexOf(')');
                    // we got them!!!
                    parameters = parameters.Remove(StartPosition);

                    StartPosition = Output_HTML_Code.IndexOf("%include(");
                    // delete them from the original HTML_Code
                    Output_HTML_Code = Output_HTML_Code.Remove(Output_HTML_Code.IndexOf("%include("), 11 + parameters.Length);
                    Output_HTML_Code = Output_HTML_Code.Insert(StartPosition, "%include_template%");
                    #endregion

                    String newTemplate = "";

                    if (LoopDetection.Contains(parameters))
                    {
                        ConsoleOutputLogger.WriteLine("%include%-Parser: possible loop found for " + parameters);
                    }
                    else
                    {

                        // this is just to make sure that this could not be used as a DoS attack vector
                        if (LoopDetection.Count == 500)
                        {
                            LoopDetection.RemoveAt(0);
                        }

                        LoopDetection.Add(parameters);

                        #region read the to be included template file
                        try
                        {
                            using (StreamReader sr = File.OpenText(parameters))
                            {
                                String buffer;

                                while ((buffer = sr.ReadLine()) != null)
                                {
                                    newTemplate = newTemplate + buffer;
                                    // read...
                                }
                                sr.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            ConsoleOutputLogger.WriteLine("%include%-Parser: " + e.Message);
                            // delete the include_template...
                            Output_HTML_Code = Output_HTML_Code.Replace("%include_template%", "");
                        }
                        #endregion

                        Output_HTML_Code = Output_HTML_Code.Replace("%include_template%", newTemplate);
                    }
                }
                catch (Exception e)
                {
                    ConsoleOutputLogger.WriteLine("%include%-Parser: " + e.Message);
                }
            }
            #endregion

            #region %querystring%
            // first find the placeholder
            while (Output_HTML_Code.Contains("%querystring%"))
            {
                Output_HTML_Code = Output_HTML_Code.Replace("%querystring%", Querystring);
            }
            #endregion

            #region %console_output%
            // first find the placeholder
            while (Output_HTML_Code.Contains("%console_output%"))
            {
                Output_HTML_Code = Output_HTML_Code.Replace("%console_output%", Template_Console_Output());
            }
            #endregion

            #region %build_version%
            while (Output_HTML_Code.Contains("%build_version%"))
            {
                Output_HTML_Code = Output_HTML_Code.Replace("%build_version%", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }

            #endregion

            #region Reflection Settings Listing
            while (Output_HTML_Code.Contains("%settings_listing%"))
            {
                StringBuilder Output = new StringBuilder();

                foreach(String blah in SettingsNames)
                {
                    Output.AppendLine(blah);
                }


                Output_HTML_Code = Output_HTML_Code.Replace("%settings_listing%", Output.ToString());
            }
            #endregion

            #region Reflection Settings
            foreach (String SettingsName in SettingsNames)
            {
                while (Output_HTML_Code.Contains("%"+SettingsName+"%"))
                {
                    Output_HTML_Code = Output_HTML_Code.Replace("%" + SettingsName + "%", internalSettings.GetSettingsValue(SettingsName));
                }
                while (Output_HTML_Code.Contains("%" + SettingsName + ".bool%"))
                {
                    String Value = internalSettings.GetSettingsValue(SettingsName);

                    if (Value.ToUpper() == "FALSE")
                        Value = "off";
                    else
                        Value = "on";

                    Output_HTML_Code = Output_HTML_Code.Replace("%" + SettingsName + ".bool%", Value);
                }
                while (Output_HTML_Code.Contains("%" + SettingsName + ".!bool%"))
                {
                    String Value = internalSettings.GetSettingsValue(SettingsName);

                    if (Value.ToUpper() == "FALSE")
                        Value = "on";
                    else
                        Value = "off";

                    Output_HTML_Code = Output_HTML_Code.Replace("%" + SettingsName + ".!bool%", Value);
                }
            }
            #endregion

            #endregion

            #endregion

            return Output_HTML_Code;
        }
        #endregion

        #region HTML Code Generators

        #region ConsoleOutput
        /// <summary>
        /// this generates the Record Listing HTML Sourcecode
        /// </summary>
        /// <returns>Record Listing Sourcecode</returns>
        String Template_Console_Output()
        {
            StringBuilder Output = new StringBuilder();

            String[] ConsoleOutput = ConsoleOutputLogger.GetLoggedLines();

            Output.Append("<pre>");

            foreach (String line in ConsoleOutput)
            {
                Output.Append(line + "\n");
            }
            Output.Append("</pre>");
            return Output.ToString();
        }
        #endregion

        #endregion
    }
}
