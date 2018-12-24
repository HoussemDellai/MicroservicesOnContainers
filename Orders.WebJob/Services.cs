using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.WebJob
{
    /// <summary>
    /// Sample services used to demonstrate DI capabilities
    /// </summary>
    public interface ISampleServiceA
    {
        void DoIt();
    }

    public class SampleServiceA : ISampleServiceA
    {
        private readonly ILogger _logger;

        public SampleServiceA(ILogger<SampleServiceA> logger)
        {
            _logger = logger;
        }

        public void DoIt()
        {
            _logger.LogInformation("SampleServiceA.DoIt invoked!");
        }
    }

    public interface ISampleServiceB
    {
        void DoIt();
    }

    public class SampleServiceB : ISampleServiceB
    {
        private readonly ILogger _logger;

        public SampleServiceB(ILogger<SampleServiceB> logger)
        {
            _logger = logger;
        }

        public void DoIt()
        {
            _logger.LogInformation("SampleServiceB.DoIt invoked!");
        }
    }
}
