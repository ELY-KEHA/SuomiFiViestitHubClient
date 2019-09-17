using System;
using System.Collections.Generic;
using System.Net.Http;
using Keha.SuomiFiViestitHub.Client;
using Newtonsoft.Json;

namespace Keha.SuomiFiViestitHub.ConsoleTester
{
    /// <summary>
    /// Example console testing app for trying out the library in production environment.
    /// Not very sophisticated, just for demoing purposes.
    /// </summary>
    class Program
    {
        private static HubClient _client;

        static int Main(string[] args)
        {
            if (args.Length != 7)
            {
                // TODO: query each prop from user when they are needed
                Console.WriteLine("Please enter correct arguments:");
                Console.WriteLine("1) Hub Url 2) Hub Port 3) Sender name 4) suomi.fi/Viestit sender account id 5) Receiving persons SSN 6) Unique messageId 7) Message text");
                return 1;
            }

            try
            {
                _client = new HubClient(new ClientConfiguration
                {
                    HubUrl = args[0],
                    HubPort = args[1],
                    CallerName = args[2],
                    ViestitAccountId = args[3]
                });

                // Make the calls
                GetState();
                if (GetCustomerAccount(args[4]))
                {
                    SendMessage(args[4], args[5], args[6]);
                    SendPrintableMessage(args[4], args[5], args[6]);
                }
                else
                {
                    Console.WriteLine("--- Cannot continue to send test message if test customer does not have an account ---");
                }

                Console.WriteLine(" * Press enter to terminate program *");
                Console.ReadLine();
                return 0;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                return 1;
            }
        }

        static void GetState()
        {
            Console.WriteLine("--- Getting state ---");
            var state = _client.GetViestitServiceState().GetAwaiter().GetResult();
            Console.WriteLine(JsonConvert.SerializeObject(state) + Environment.NewLine);
        }

        static bool GetCustomerAccount(string ssn)
        {
            Console.WriteLine("--- Checking if given customer has an account ---");
            var returnBool = _client.CustomerHasAccount(ssn).GetAwaiter().GetResult();
            Console.WriteLine(returnBool + Environment.NewLine);
 
            return returnBool;
        }

        static void SendMessage(string ssn, string msgId, string msgText)
        {
            Console.WriteLine("--- Sending the test message to Viestit-Service ---");
            Console.WriteLine(" * Press enter to confirm action *");
            Console.ReadLine();
            var msg = new ViestitMessage
            {
                SocialSecurityNumber = ssn,
                Id = msgId,
                Topic = "TestiViesti",
                SenderName = "HubClientTestingSoftware",
                Text = msgText,
                MsgId = "123",
                Links = new List<ViestitMessageLink>(),
                Files = new List<ViestitMessageFile>()
            };

            // Hardcoded links and files just to make sure everything works
            msg.Links.Add(new ViestitMessageLink
            {
                Description = "Linkki asiointiin",
                Url = "www.google.fi"
            });
            msg.Links.Add(new ViestitMessageLink
            {
                Description = "Toinen linkki asiointiin",
                Url = "www.google.fi"
            });
            msg.Files.Add(new ViestitMessageFile
            {
                Content = Base64Files.PNG,
                ContentType = "image/jpeg",
                Name = "Kuva.jpg",
                Size = 16
            });

            var msgState = _client.SendMessageToViestit(new List<ViestitMessage>{ msg }).GetAwaiter().GetResult();
            Console.WriteLine(JsonConvert.SerializeObject(msgState[0]) + Environment.NewLine);
        }

        static void SendPrintableMessage(string ssn, string msgId, string msgText)
        {
            Console.WriteLine("--- Sending the printable test message to Viestit-Service (but not to printing) ---");
            Console.WriteLine(" * Press enter to confirm action *");
            Console.ReadLine();

            var msg = new PrintableViestitMessage
            {
                SocialSecurityNumber = ssn,
                Id = msgId + "-1",
                Topic = "TestiViesti",
                SenderName = "HubClientTestingSoftware",
                Text = msgText,
                MsgId = "123",
                File = new ViestitMessageFile // Hardcoded file just to make sure everything works
                {
                    Content = Base64Files.PDF,
                    ContentType = "application/pdf",
                    Name = "PDF-kirje.pdf",
                    Size = 40
                },
                TestingOnlyDoNotSendPrinted = true, // NOTE: Very important to keep this true
                Address = new AddressInformation
                {
                    RecipientName = "Testi Vastaanottaja",
                    StreetAddress = "Testi Osoite Ei Ole Olemassa",
                    PostalCode = "00000",
                    City = "Testikaupunki",
                    CountryCode = "FI"
                },
                PrintingProvider = "Edita" // NOTE: Has to be a valid provider
            };

            var msgState = _client.SendPrintableMessageToViestit(new List<PrintableViestitMessage> { msg }).GetAwaiter().GetResult();
            Console.WriteLine(JsonConvert.SerializeObject(msgState[0]) + Environment.NewLine);
        }
    }
}
