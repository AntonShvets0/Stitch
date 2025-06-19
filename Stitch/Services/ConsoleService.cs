namespace Stitch.Services;

public class ConsoleService
{
    private readonly string _stitchArt = """

                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠏⠀⠀⠀⠀⠀⠀⠀⣼⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⠀⠀⠀⠀⠀⠀⢠⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⠀⠀⠀⠀⡞⠸⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣴⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠤⠀
                                         ⠀⠀⠀⠀⠀⠀⢀⣶⠀⠀⠸⡄⠀⠀⠀⠀⠀⢰⠃⠀⡇⠀⠀⠀⢀⣤⣔⣚⣛⠛⠟⠛⠛⠉⠉⠙⠛⠻⢭⡿⣶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⡄
                                         ⠀⠀⣾⣦⣀⢰⡿⢿⡆⣀⣠⣷⡄⠀⠀⠀⠀⣼⠀⠀⢹⣀⣴⡿⠟⠋⠉⠉⠉⠛⠷⡄⠀⠀⠀⠀⠤⠤⢤⣙⢦⡉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⠀⠀⠃
                                         ⠀⠀⠸⣅⠈⠛⠁⠈⠳⠏⢙⡇⠙⣦⣀⠀⠀⢿⠀⢀⡞⢡⠞⠀⠀⠀⠀⠀⠀⠀⠀⣷⠀⠀⠀⠀⠀⠀⠀⠀⠙⠳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡰⠋⠀⠀⠀⠀⠀⢠⠀
                                         ⢠⣤⡤⡟⠀⡼⠋⢹⠀⠀⣿⠀⠀⠘⣎⠓⠦⢼⣤⠎⢠⡏⠀⣠⣾⣿⣿⢶⡄⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⡄⠀⠀⠀⠀⠀⠀⠀⢀⡞⠁⠀⠀⠀⠀⠀⠀⡌⠀
                                         ⠀⠻⢧⣴⡀⠛⠦⠼⠂⠰⠛⠲⢤⡀⠘⢦⡀⢠⠏⠀⢸⠀⣰⣿⣿⣿⣇⣼⡇⠀⡾⠀⠀⠀⢀⠀⣀⠀⠀⠀⢀⣐⡲⣷⠀⠀⠀⠀⠀⠀⢠⠏⠀⠀⠀⠀⠀⠀⠀⣸⠁⠀
                                         ⠀⠀⠀⠈⠛⢶⠶⠚⠁⠀⠀⠀⠀⠉⠳⣄⠙⡿⠀⠀⠸⡄⢿⣿⣿⣿⣿⡿⠀⡼⢁⣀⣼⣗⣚⡯⣾⣗⠀⣠⠋⠉⠻⣿⡇⠀⠀⠀⠀⣰⠃⠀⠀⠀⠀⠀⠀⠀⡰⠃⠀⠀
                                         ⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⡇⠴⢶⡆⠹⣌⣛⠿⢿⣿⡁⠞⣴⠋⠀⠀⠀⠈⠙⢾⣳⣤⠇⠀⠀⠀⢸⡇⠀⢀⡤⢺⠃⠀⠀⠀⠀⠀⠀⠀⡴⠃⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢳⠀⢸⡙⠢⣄⡉⢛⣺⠿⠃⢰⢷⡆⠀⠀⠀⠀⠀⠀⢹⣏⣴⣿⣿⣷⢸⣅⡴⠋⢠⠏⠀⠀⠀⠀⠀⠀⢀⡞⠁⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠈⢧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣇⠀⢧⠀⣀⣹⣷⡦⣄⡀⠈⢻⣧⠀⠀⠀⠀⠀⣀⣿⣿⣿⣼⣿⡟⡼⠁⠀⢠⠏⠀⠀⠀⠀⠀⠀⣠⠏⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠹⣦⡀⠀⠀⠀⠀⠀⠀⠀⠘⣆⠈⢿⣡⡀⠀⠙⠶⠬⢿⠲⢯⣷⣤⣤⣶⠿⣿⠿⣿⣿⣿⡟⢱⠃⠀⡴⠋⠀⠀⠀⣄⣀⣠⠞⠁⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠦⣄⡀⠸⡆⠀⠀⢀⣼⡷⣄⠙⢿⡒⣆⠀⠀⠘⣟⠒⠮⣝⠲⡦⣄⡘⢦⣉⣋⣉⡠⢋⣠⣞⠁⠀⠀⠀⣼⠁⠈⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⣇⠀⣰⢫⠎⠀⠈⠓⠦⣍⠻⠤⣄⣀⠈⠀⠀⣸⡆⠙⠒⣛⢶⣌⡽⠛⠛⠫⠦⠚⠛⠛⠋⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣶⠇⡏⠀⠀⠀⢀⣤⠎⠉⠒⠦⣬⣉⠓⠛⠓⠛⢋⣭⣵⡊⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⢰⠇⠀⠀⢻⣷⠏⠀⠀⠀⠀⠀⢨⠿⡍⠉⠉⠉⠀⠀⠙⠲⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡼⢸⠀⠀⠀⢸⣏⠀⠀⠀⠀⠀⢰⠋⠀⢹⠀⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠇⢸⠀⠀⠀⠈⠛⠀⠀⠀⠀⠀⡏⠀⠀⠈⢧⣀⣠⠤⠒⠲⢤⡀⠀⠸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠏⠀⡼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⣤⠟⠁⠀⠀⠀⠀⠉⠀⠀⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡟⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢧⠀⠀⡼⠁⠀⠀⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣆⠀⡇⠀⠀⠀⠀⠀⠀⠀⢀⣴⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣆⢧⡀⠀⠀⠀⠤⢖⣚⣿⣾⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⣧⠀⢹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⠀⠙⠦⣄⣀⡀⠈⠋⢈⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                         ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⠏⠀⠘⣆⠘⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⠀⠀⠀⠀⠀⣯⣉⢉⢉⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀

                                         """;

    public void ShowWelcome()
    {
        Console.Clear();
        SetColor(ConsoleColor.Blue);
        
        Console.WriteLine(_stitchArt);
        
        Console.WriteLine();
        SetColor(ConsoleColor.Cyan);
        Console.WriteLine("  ╔════════════════════╗");
        Console.WriteLine("  ║  STITCH FILE TOOL  ║");
        Console.WriteLine("  ╚════════════════════╝");
        Console.WriteLine();
        ResetColor();
    }
    
    public void ShowCommand(string command, string description)
    {
        SetColor(ConsoleColor.Cyan);
        Console.Write($"  {command,-20}");
        SetColor(ConsoleColor.White);
        Console.WriteLine($" - {description}");
        ResetColor();
    }

    public void ShowSuccess(string message)
    {
        SetColor(ConsoleColor.Green);
        Console.WriteLine($"✓ {message}");
        ResetColor();
    }

    public void ShowError(string message)
    {
        SetColor(ConsoleColor.Red);
        Console.WriteLine($"✗ {message}");
        ResetColor();
    }

    public void ShowInfo(string message)
    {
        SetColor(ConsoleColor.Yellow);
        Console.WriteLine($"ℹ {message}");
        ResetColor();
    }

    public void ShowProgress(string message)
    {
        SetColor(ConsoleColor.Magenta);
        Console.Write($"⟳ {message}");
        ResetColor();
    }

    public void ClearProgress()
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
    }

    public void SetColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }
}