using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XsdDropHandler
{
    public class XsdValidator
    {
        public void Validate(string xsdPath, IEnumerable<string> xmlPaths)
        {
            //  Create XML reader settings that force schema validation against the specified xsd file.
            var readerSettings = new XmlReaderSettings();

            //  Try and add the schema, it may be wrong itself.
            validationMessages[xsdPath] = new List<ValidationMessage>();
            try
            {
                readerSettings.Schemas.Add(null, xsdPath);
            }
            catch (Exception)
            {
                validationMessages[xsdPath].Add(new ValidationMessage(xsdPath, ValidationType.Error, "The schema does not appear to be valid."));
                return;
            }
            validationMessages[xsdPath].Add(new ValidationMessage(xsdPath, ValidationType.Success, "Successfully loaded schema from " + 
                Path.GetFileNameWithoutExtension(xsdPath) + "."));

            readerSettings.ValidationType = System.Xml.ValidationType.Schema;
            readerSettings.ValidationEventHandler += readerSettings_ValidationEventHandler;

            //  Go through each xml path.
            foreach (var xmlPath in xmlPaths)
            {
                //  Store the current file.
                currentXmlFile = xmlPath;

                //  Create the entry in the dictionary for validation errors.
                validationMessages[currentXmlFile] = new List<ValidationMessage>();

                //  Create a reader for the file.
                using(var xmlStream = new FileStream(xmlPath, FileMode.Open))
                {
                    try
                    {

                        using (var reader = XmlReader.Create(xmlStream, readerSettings))
                        {
                            //  Read the data.
                            while (reader.Read())
                            {
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        validationMessages[currentXmlFile].Add(new ValidationMessage(xmlPath, ValidationType.Error, exception.Message));
                    }

                    validationMessages[currentXmlFile].Add(new ValidationMessage(currentXmlFile, ValidationType.Success, "Validation Complete."));
                }
            }
        }

        void readerSettings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            //  Update the dictionary.
            validationMessages[currentXmlFile].Add(new ValidationMessage(currentXmlFile, e));
            
        }

        private string currentXmlFile;

        private readonly Dictionary<string, List<ValidationMessage>> validationMessages = new Dictionary<string, List<ValidationMessage>>();

        public Dictionary<string, List<ValidationMessage>> ValidationMessages { get { return validationMessages; } }
    }

    public class ValidationMessage
    {
        public ValidationMessage(string xmlFilePath, ValidationType severity, string message)
        {
            Message = message;
            ValidationType = severity;
            XmlFilePath = xmlFilePath;
        }

        public ValidationMessage(string xmlFilePath, ValidationEventArgs args)
        {
            XmlFilePath = xmlFilePath;
            Message = args.Message;

            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    ValidationType = ValidationType.Error;
                    break;
                case XmlSeverityType.Warning:
                    ValidationType = ValidationType.Warning;
                    break;
                default:
                    ValidationType = ValidationType.Success;
                    break;
            }
        }

        public string XmlFilePath { get; private set; }
        public ValidationType ValidationType { get; private set; }
        public string Message { get; private set; }
    }

    public enum ValidationType
    {
        Success = 0,
        Warning = 1,
        Error = 2
    }
}
