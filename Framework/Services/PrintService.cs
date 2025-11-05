using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using static Framework.Constants.RegexConstants;

namespace Framework.Services
{
    public class PrintService
    {
        public static async Task PrintOverTcp_Ip(string printerIp = "192.168.1.100", int printerPort = 9100, string message = "Default")
        {
            try
            {
                byte[] initializePrinter = new byte[]
                {
                    0x1B, 0x40,
                    0x1B, 0x44, 0x02, 0x04, 0x06, 0x08, 0x04, 0x00,
                    0x1D, 0x21, 0x10
                };
                byte[] cutCommand = new byte[]
                {
                    0x1B, 0x64, 0x09,
                    0x1D, 0x56, 0x00,
                    0x0C,
                    0x1B, 0x40
                };
                List<string> parsedMessage = ParseHtmlBody(message);
                foreach (string currentMessage in parsedMessage)
                {
                    byte[] msg = Encoding.ASCII.GetBytes(currentMessage);
                    byte[] fullPrintMsg = new byte[initializePrinter.Length + msg.Length + cutCommand.Length];
                    Array.Copy(initializePrinter, 0, fullPrintMsg, 0, initializePrinter.Length);
                    Array.Copy(msg, 0, fullPrintMsg, initializePrinter.Length, msg.Length);
                    Array.Copy(cutCommand, 0, fullPrintMsg, initializePrinter.Length + msg.Length, cutCommand.Length);
                    using (TcpClient client = new TcpClient())
                    {
                        Console.WriteLine("Connecting to printer...");
                        await client.ConnectAsync(printerIp, printerPort);
                        client.LingerState = new LingerOption(true, 0);
                        using (NetworkStream stream = client.GetStream())
                        {
                            await stream.WriteAsync(fullPrintMsg, 0, fullPrintMsg.Length);
                            await stream.FlushAsync();
                            await Task.Delay(1500);
                            Console.WriteLine("Comanda impresa exitosamente");
                        }
                        Console.WriteLine("Closing connection...");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected printing behaviour\nInitializing error description:");
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Finalizing error description.");
            }
        }

        private static List<string> ParseHtmlBody(string body)
        {
            var listOfMessages = new List<string>();
            var listOfOrders = Regex.Split(body, HrTagRegex);
            foreach (string order in listOfOrders)
            {
                var regexMatches = Regex.Matches(order, DivTagRegex);
                var ticketLines = Regex.Split(order, DivTagRegex);
                var listIndexes = new List<string>();
                if (regexMatches.Count > 0)
                {
                    foreach (var match in regexMatches)
                    {
                        string matchString = match.ToString() ?? string.Empty;
                        string matchValue = Regex.Match(matchString, @"\d(?=rem)").Value;
                        string indexToAdd = string.IsNullOrEmpty(matchValue) ? "0" : matchValue;
                        listIndexes.Add(indexToAdd);
                    }
                    for (int i = 0; i < ticketLines.Count(); i++)
                    {
                        string removedHtml = Regex.Replace(ticketLines[i], @"<.*?>", string.Empty);
                        string addedLineFeeds = Regex.Replace(removedHtml, @"-", "\n-");
                        string parsedArrows = Regex.Replace(addedLineFeeds, @"&gt;", ">");
                        ticketLines[i] = parsedArrows;
                    }
                    for (int i = 1; i < ticketLines.Count(); i++)
                    {
                        var tabsToAdd = new List<string>();
                        for (int j = 0; j < int.Parse(listIndexes[i - 1]); j++)
                        {
                            tabsToAdd.Add("\t");
                        }
                        string startLineTabs = ticketLines[i].Insert(0, string.Join(string.Empty, tabsToAdd));
                        ticketLines[i] = Regex.Replace(startLineTabs, @"\n", $"\n{string.Join(string.Empty, tabsToAdd)}");
                    }
                    string fullTicketString = string.Join('\n', ticketLines);
                    Console.WriteLine(fullTicketString);
                    listOfMessages.Add(fullTicketString);
                }
            }
            return listOfMessages;
        }
    }
}