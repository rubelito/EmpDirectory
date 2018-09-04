using System;
using System.Collections.Generic;
using System.IO;
using BCS.Application.Domain;
using BCS.Application.Entity;
using CsvHelper;

namespace BCS.Helper
{
    public class CSVMaker
    {
        readonly IEmployeeRepositoy _empRepo;

        public CSVMaker(IEmployeeRepositoy empRepo)
        {
            _empRepo = empRepo;
        }

        public string MakeEmployeeSCV()
        {
            List<MainUser> users = _empRepo.GetAllUser();

            using(var memoryStream = new MemoryStream())
            using(var streamWriter = new StreamWriter(memoryStream))
            using(var csv = new CsvWriter(streamWriter))
            {
                csv.WriteRecords(users);
                streamWriter.Flush();

                memoryStream.Position = 0;
                using(var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
