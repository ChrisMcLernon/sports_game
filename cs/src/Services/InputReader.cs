namespace sports_game.src.Services
{
    static public class InputReader
    {
        static public string ReadText(string prompt = "")
        {
            while (true)
            {
                Console.Write("\n" + prompt);
                string? input = Console.ReadLine();
                
                if (input == "" || input == null)
                {
                    Console.WriteLine("No input is invalid");
                }
                else
                {
                    return input;
                }
            }
        }
    }
}