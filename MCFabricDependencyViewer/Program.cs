using ANSIConsole;

namespace MCFabricDependencyViewer
{
    internal class Program
    {
        const string path = @"";
        
        static void Init()
        {
            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;//if we don't do this the entire strings being displayed on the entire app might not work

            Menu.Init();
        }

        static void Main(string[] args)
        {
            Init();

            do
            {
                UI.WriteLine(Menu.HeaderBase);

                UI.WriteLines(UI.GetBody(path, UI.Get_user_selection()));

            } while (true);
        }
    }
}