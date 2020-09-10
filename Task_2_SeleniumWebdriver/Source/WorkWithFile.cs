using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace App.Test.File
{
    public static class WorkWithFile<T>
    {
        public static void WritingCsv(string path, IEnumerable<T> data) {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture)){                        
                        foreach (T item in data) {
                            csvWriter.WriteField<T>(item);
                            csvWriter.NextRecord();
                        }                        
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public static IEnumerable<T> ReadingCsv(string path) {
            List<T> result = new List<T>();
            T value;
            using(StreamReader streamReader = new StreamReader(path, Encoding.UTF8)) { 
                using(CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture)) {
                    csvReader.Configuration.HasHeaderRecord = false;
                    while (csvReader.Read()) {
                        for (int i = 0; csvReader.TryGetField<T>(i, out value); i++) {
                            result.Add(value);
                        }
                    }
                }
                return result;
            }
        }
    }
}
