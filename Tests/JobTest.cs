using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
  class JobTest : IJob
  {
    public void Execute(IJobExecutionContext context)
    {
      Console.WriteLine("Job coming");
    }
  }
}
