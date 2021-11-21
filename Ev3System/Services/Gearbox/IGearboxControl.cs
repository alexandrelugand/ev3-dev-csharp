using EV3.Dev.Csharp.Services.Remoting;
using System;

namespace Ev3System.Services.Gearbox
{
    public interface IGearboxControl : IRemoteService, IDisposable
    {
        void Prepare();
        void Unprepare();

        int Gear { get; }

        void GearUp();
        void GearDown();
    }
}
