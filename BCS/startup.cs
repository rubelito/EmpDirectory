using System;
//using Microsoft.AspNet.SignalR;
using Owin;

namespace BCS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app){
            //HubConfiguration hub = new HubConfiguration();
            //hub.EnableJSONP = true;
            app.MapSignalR();
        }
    }
}
