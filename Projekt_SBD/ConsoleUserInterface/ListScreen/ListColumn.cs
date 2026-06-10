namespace Projekt_SBD.ConsoleUserInterface.ListScreen
{
    public class ListColumn<T>
    {
        public string Header { get; set; }
        public Func<T, object?> GetValueFunc { get; set; }

        public ListColumn(string header, Func<T, object?> getValueFunc)
        {
            Header = header;
            GetValueFunc = getValueFunc;
        }
    }
}
