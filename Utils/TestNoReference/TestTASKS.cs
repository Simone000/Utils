using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedUtilsNoReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNoReference
{
    [TestClass]
    public class TestTASKS
    {
        private async Task<int> NotImplementedExceptionMethodAsync()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task TestRetryOnFaultAsync()
        {
            await TASKS.RetryOnFaultAsync(() => NotImplementedExceptionMethodAsync());
        }
    }
}
