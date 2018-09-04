using System;
using System.Collections.Generic;
using System.IO;
using BCS.Models;

namespace BCS.Helper
{
    public class CountryServiceHelper
    {
        public CountryServiceHelper()
        {
        }

        public List<State> GetState(string path)
        {
            var strStates = File.ReadAllText(path);
            List<State> states = Newtonsoft.Json.JsonConvert.DeserializeObject<List<State>>(strStates);

            return states;
        }
    }
}
