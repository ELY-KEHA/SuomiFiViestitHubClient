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
            if (args.Length > 7)
            {
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
                // Base64 encoded Suomi.fi logo
                Content = "/9j/4AAQSkZJRgABAQEAkACQAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAAfAB8DASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD8r69K+On7HnxQ/Zk0HQ9T+IPgfX/CFl4lVm0t9Ut/IN4FVGbap+b5RImcgY3DNea19r/8Fav2iv2oPjn4c8BW/wC0R4CbwXZ6PLeDw+z6HJppuWdYBOBvdt+0JD9Nw9a/p3EV60MRSpw5eWV73eui05V18+yPwqjSpyo1JyvdWtZaavW76eR57b/8Edf2n7u3SWL4KeOJYpVDo6WilXU8gg7uRXzffWU2mXs1tcRSQXFu7RSxuu1o2U4KkdiCK/ZDR/8Agoj/AMFMLXSLWOy+B8hs44UWAp4JuHUxhQFw3mnIxjnNfj/45ur298baxNqUKW2oy3073UKfdilMjF1HJ4DZHU9OprgybG4yvKaxXs9LW5JX77nXmWFw9FRdDn135lb7jKr9jv8Ag6j+Knhf4h/Dz4BweH/Evh/XpbKTVzcJpuow3ZhBisAC3lscAkHBPXFfj5rmi3PhvW7zTr2Pybywne2nj3BvLkRirDIJBwQeQcVUrpxWWxxGLoYzmt7Lm078ysY4fHOjh6uGt/Et8uV3P0h03/g6Q/aQ0ixt7aDSPhYsFsixRodFujhVAAGftWeg9a/OrxDrk3ibX77UrjYLjULiS5l2DC73YscD0yap0VtgsrwmEbeGpqN97dTPE4/EYhJV5uVtrn//2Q==",
                ContentType = "image/jpeg",
                Name = "Kuva.jpg",
                Size = 1
            });

            var msgState = _client.SendMessageToViestit(new List<ViestitMessage>{ msg }).GetAwaiter().GetResult();
            Console.WriteLine(JsonConvert.SerializeObject(msgState[0]) + Environment.NewLine);
        }
    }
}
