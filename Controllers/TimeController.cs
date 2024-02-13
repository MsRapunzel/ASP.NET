using System;
using labWork.Interfaces;

namespace labWork.Controllers
{
	public class TimeController : ITimeService
	{
        public DateTime GetTime() => DateTime.Now;
	}
}

