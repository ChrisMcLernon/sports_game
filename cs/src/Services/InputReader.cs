namespace sports_game.src.Services
{
    public class InputReader
    {
        public static string ReadText(string prompt = "")
        {
            while (true)
            {
                Console.Write(prompt);
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