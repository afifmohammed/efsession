using System;

namespace efsession
{
    public interface ISession : IStore, IDisposable
    {
        void Commit();
    }
}