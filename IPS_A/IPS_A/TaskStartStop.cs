using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IPS_A
{
    internal class TaskStartStop : IDisposable
    {
        CancellationTokenSource CancellationTokenSource;
        CancellationToken token;

        public TaskStartStop()
        {
            CancellationTokenSource = new CancellationTokenSource();
            token = CancellationTokenSource.Token;
        }

        public void Start()
        {
            Random random = new Random();

            Task task = new Task(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    M.ScrollDown();
                    Thread.Sleep(5);
                    M.ScrollUp();
                    Thread.Sleep(random.Next(30000,300000));
                }

            }, token);

            task.Start();
        }
        public void Stop()
        {
            CancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            CancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
        
    }
}
