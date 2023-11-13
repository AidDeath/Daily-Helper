using Daily_Helper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Daily_Helper.Services
{
    public class RoutinesSaveLoadService
    {
        private RoutineTestsProvider _routinesProvider;
        public RoutinesSaveLoadService(RoutineTestsProvider routinesProvider)
        {
            _routinesProvider = routinesProvider;
        }


        /// <summary>
        /// Serializes all current routines to JSON 
        /// </summary>
        /// <returns>List of serialized routines</returns>
        public List<SerializedRoutine> SerializeRoutines()
        {
            List<SerializedRoutine> serializedRoutines = new();
            foreach (var current in _routinesProvider.Routines)
            {
                serializedRoutines.Add(current.GetSerialized());
            }

            return serializedRoutines
                //.OrderBy(r => r.Type.Name)  // sorting don't needed?
                .ToList();
        }


        /// <summary>
        /// Serialize and save routinesProvider to the file near executable
        /// </summary>
        /// <returns></returns>
        public async Task SaveFileOnExit(string? savePath = null)
        {
            //List<SerializedRoutine> serializedRoutines = new();
            //foreach (var current in _routinesProvider.SelectedRoutines)
            //{
            //    serializedRoutines.Add(current.GetSerialized());
            //}

            //serializedRoutines = serializedRoutines.OrderBy(r => r.Type.Name).ToList();

            await SaveToFile($"{Environment.CurrentDirectory}/routines.dhlist"); 
        }

        //TODO: add exceptions for access
        public async Task SaveToFile(string savePath)
        {
            var serializedRoutines = SerializeRoutines();

            string jsonText = "";
            
            foreach (var current in serializedRoutines)
            {
                jsonText += ($"--RECORD--{current?.Type?.Name}--TYPE--");
                jsonText +=current?.JsonString;
            }

            await File.WriteAllTextAsync(savePath, jsonText);
                          
        }


        public async Task<List<SerializedRoutine>?> LoadOnStartUp()
        {

            return await LoadFromFile($"{Environment.CurrentDirectory}/routines.dhlist");

        }

        public async Task<List<SerializedRoutine>?> LoadFromFile(string loadPath)
        {
            //string? fileContent;
            //List<SerializedRoutine>? result = new();

            //using (var fileStream = new FileStream($"{Environment.CurrentDirectory}/routines.dat", FileMode.OpenOrCreate))
            //using (var sr = new StreamReader(fileStream))
            //{
            //    fileContent = await sr.ReadToEndAsync();
            //}

            //string[] possibleTypes = { "FileShareRoutine", "PingRoutine", "ProcessStateRoutine", "ServiceStateRoutine", "ProcessStateRoutine", "DriveFreeSpaceRoutine", "ConnPortRoutine" };

            string? fileContent;

            using (var fileStream = new FileStream(loadPath, FileMode.OpenOrCreate))
                using (var sr = new StreamReader(fileStream))
                {
                    fileContent = await sr.ReadToEndAsync();
                }

            
            List<SerializedRoutine>? result = new();

            foreach (var entry in fileContent.Split("--RECORD--", StringSplitOptions.RemoveEmptyEntries))
            {
                var record = entry.Split("--TYPE--");

                switch (record[0])
                {
                    case "FileShareRoutine":
                        result.Add(new() { Type = typeof(FileShareRoutine), JsonString = record[1] });
                        break;
                    case "PingRoutine":
                        result.Add(new() { Type = typeof(PingRoutine), JsonString = record[1] });
                        break;
                    case "ProcessStateRoutine":
                        result.Add(new() { Type = typeof(ProcessStateRoutine), JsonString = record[1] });
                        break;
                    case "ServiceStateRoutine":
                        result.Add(new() { Type = typeof(ServiceStateRoutine), JsonString = record[1] });
                        break;
                    case "DriveFreeSpaceRoutine":
                        result.Add(new() { Type = typeof(DriveFreeSpaceRoutine), JsonString = record[1] });
                        break;
                    case "ConnPortRoutine":
                        result.Add(new() { Type = typeof(ConnPortRoutine), JsonString = record[1] });
                        break;
                    case "FolderLastChangedRoutine":
                        result.Add(new() { Type = typeof(FolderLastChangedRoutine), JsonString = record[1] });
                        break;
                    default:
                        break;

                }
            }



            return result;

        }
    }
}
