using Daily_Helper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
        /// Serialize and save routinesProvider to the file near executable
        /// </summary>
        /// <returns></returns>
        public async Task SerializeAndSave()
        {
            List<SerializedRoutine> serializedRoutines = new();
            foreach (var current in _routinesProvider.Routines)
            {
                serializedRoutines.Add(current.GetSerialized());
            }

            serializedRoutines = serializedRoutines.OrderBy(r => r.Type.Name).ToList();


            using (var fileStream = new FileStream($"{Environment.CurrentDirectory}/routines.dat", FileMode.Create))
            using (var sw = new StreamWriter(fileStream))
            {
                foreach (var current in serializedRoutines)
                {
                    await sw.WriteLineAsync($"--RECORD--{current?.Type?.Name}--TYPE--");
                    await sw.WriteLineAsync(current?.JsonString);

                }

            }
        }

        public async Task<List<SerializedRoutine>?> LoadAndDeserialize()
        {
            string? fileContent;
            List<SerializedRoutine>? result = new();

            using (var fileStream = new FileStream($"{Environment.CurrentDirectory}/routines.dat", FileMode.OpenOrCreate))
            using (var sr = new StreamReader(fileStream))
            {
                fileContent = await sr.ReadToEndAsync();
            }

            string[] possibleTypes = { "FileShareRoutine", "PingRoutine", "ProcessStateRoutine", "ServiceStateRoutine", "ProcessStateRoutine", "DriveFreeSpaceRoutine", "ConnPortRoutine" };

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
                    default:
                        break;

                }
            }



            return result;

        }
    }
}
