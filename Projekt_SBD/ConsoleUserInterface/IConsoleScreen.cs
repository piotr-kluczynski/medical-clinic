using Projekt_SBD.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_SBD.ConsoleUserInterface
{
    public interface IConsoleScreen
    {
        ScreensEnum Id { get; }
        ScreensEnum Run();
    }
}
