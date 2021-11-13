using EV3.Dev.Csharp.Services.Remoting;
using System;

namespace Ev3System.Services.Engine
{
    public interface IEngineControl : IRemoteService, IDisposable
    {
        void Prepare();
        void Unprepare();

        int Speed { get; set; }
        int Duty { get; set; }

        void Idle();
        void Start();
        void Stop(bool brake = false);
        void Ramp(int speed, double time);
    }
}