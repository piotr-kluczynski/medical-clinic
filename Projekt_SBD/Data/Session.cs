using Projekt_SBD.Models;

namespace Projekt_SBD.Data
{
    public static class Session
    {
        public static Patient? CurrentPatient { get; set; }

        public static Worker? CurrentWorker { get; set; }

        public static void Logout()
        {
            CurrentPatient = null;
            CurrentWorker = null;
        }
    }
}
