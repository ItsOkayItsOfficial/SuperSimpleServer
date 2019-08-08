using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SuperSimpleServer
{

    class Program
    {

        // Page Builder defines how the file is "read" and "returned" to the browser/requestor
        static void pageBuilder(string page, HttpListenerResponse res)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(page);

            res.ContentLength64 = buffer.Length;
            Stream st = res.OutputStream;
            st.Write(buffer, 0, buffer.Length);
        }

        // Main - collection for CL arguements that will NOT be analyzed.
        static void Main(string[] args)
        {
            // Check if HttpListener is NOT supported 
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("\nWindows XP SP2 or Server 2003 is required to use this Http Listener.");
                return;
            }

            // Get domain input
            Console.WriteLine("Enter domain to listen at: ");
            string domain = Console.ReadLine();

            // Get port input
            Console.WriteLine("\nEnter port to listen at: ");
            string port = Console.ReadLine();

            // Define address to listen at
            string prefix = "http://" + domain + ":" + port + "/";
            HttpListener server = new HttpListener();
            server.Prefixes.Add(prefix);

            // Start listener
            try
            {
                server.Start();
                Console.WriteLine("\nSimple Listener is listening at " + prefix);

                // Handle requests
                while (true)
                {
                    // Set "context" for listening
                    HttpListenerContext context = server.GetContext();
                    // Set "response" for serving
                    HttpListenerResponse response = context.Response;

                    // Defining local directory files outside of request
                    string localDir = Directory.GetCurrentDirectory();
                    string localFile = context.Request.Url.LocalPath;
                    string file = localDir + localFile + ".html";

                    // Successful Response (server)
                    try
                    {
                        // Read static file into string
                        TextReader tr = new StreamReader(file);
                        string msg = tr.ReadToEnd();

                        // Build and send page
                        Program.pageBuilder(msg, response);

                        // Log success
                        Console.WriteLine("\nSuccess! 200 - OK");
                        // Log sucess message
                        Console.WriteLine("Page for " + localFile + " done got served!");
                    }
                    // Handle File Not Found exceptions
                    catch (FileNotFoundException)
                    {

                        string notFound = "<html><header></header><body><h1>404! Page Not Found</h1><p>The page you requested could not be found</p></body></html>";
                        Program.pageBuilder(notFound, response);

                        // Log error
                        Console.WriteLine("\nError! 404 - Not Found");
                        // Log error message
                        Console.WriteLine("Request not fullfilled as the file requested (" + localFile + ") was not found");
                    }
                    // Handle any other exceptions
                    catch (Exception e)
                    {
                        string badReq = "<html><body><h1>400! Bad Request</h1><p>The server didn't recognize your request, sorry!</p></body></html>";
                        Program.pageBuilder(badReq, response);

                        // Log error
                        Console.WriteLine("\nError! 400 - Bad Request");
                        // Log error message
                        Console.WriteLine("Page could not be loaded. Server returned the following:\n" + e);
                    }

                    // For having Super Simple Server listener stop listener after a single request.
                    // context.Response.Close();
                    // Console.WriteLine("Simple Listener shutting down");
                    // break;
                }
            }
            // Handle exceptions arising from starting listener
            catch (Exception e)
            {
                // Log error
                Console.WriteLine("\nError!");
                // Log error message
                Console.WriteLine("Listener could not be started at " + prefix + ". The following error was returned:\n" + e);
            }
        }
    }
}
