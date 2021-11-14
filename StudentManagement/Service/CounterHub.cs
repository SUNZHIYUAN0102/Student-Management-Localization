using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public class CounterHub : Hub
    {
        static long counter = 0;

        public async override Task OnConnectedAsync()
        {
            counter = counter + 1;

            await this.Clients.All.SendAsync("UpdateCount", counter);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception e)
        {
            counter = counter - 1;
            await this.Clients.All.SendAsync("UpdateCount", counter);
            await base.OnDisconnectedAsync(e);
        }
    }
}
