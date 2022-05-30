using System;
using System.Collections.Generic;
using System.Text;

namespace TechTest.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
