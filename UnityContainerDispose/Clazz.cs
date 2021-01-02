using System;

namespace ConsoleApp1
{
    interface IService
    {
        Guid Guid { get; }
    }
    class Service : IService, IDisposable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public Service() => Console.WriteLine($"    Create  Service {Guid}");
        public void Dispose() => Console.WriteLine($"    Dispose Service {Guid}");
    }

    interface IReader
    {
        Guid Guid { get; }
    }
    class Reader : IReader, IDisposable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public Reader() => Console.WriteLine($"    Create  Reader {Guid}");
        public void Dispose() => Console.WriteLine($"    Dispose Reader {Guid}");
    }

    interface IWriter
    {
        Guid Guid { get; }
    }
    class Writer : IWriter, IDisposable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public Writer() => Console.WriteLine($"    Create  Writer {Guid}");
        public void Dispose() => Console.WriteLine($"    Dispose Writer {Guid}");
    }
}
