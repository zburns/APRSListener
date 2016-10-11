using System;
using System.Collections.Generic;
using System.Text;

namespace APRSListener
{
    class Program
    {
       //XXXXXX - is your FCC Callsign
       //YYYYY - is your APRS ID Designator
       //ZZZZZ - is your APRS network password (see my other program to generate one)
    
        const int FULL_APRS_FEED = 10152;
        const int MESSAGE_ONLY_FEED = 1314;
        const int USER_DEFINED_FILTER = 14580;

        static void Main(string[] args)
        {
            //Listen
            Listen();
        }

        static void Listen()
        {
            System.IO.TextWriter output = System.IO.File.CreateText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\APRS.TXT");

            System.Net.Sockets.NetworkStream stream;
            System.Net.Sockets.TcpClient tcp = new System.Net.Sockets.TcpClient("rotate.aprs.net", MESSAGE_ONLY_FEED);
            //connected, now get stream to listen to
            stream = tcp.GetStream();
            byte[] commandtoissue = System.Text.ASCIIEncoding.ASCII.GetBytes("user XXXXX-Y pass ZZZZZZ version APRSListener 1.0 filter r/41.65/-83.54/500\r\n");
            stream.Write(commandtoissue, 0, commandtoissue.Length);

            int bytesread = 0;
            byte[] readbuffer = new byte[1];
            while ((bytesread = stream.Read(readbuffer, 0, 1)) > 0)
            {
                Console.Write(System.Text.ASCIIEncoding.ASCII.GetString(readbuffer));
                output.Write(System.Text.ASCIIEncoding.ASCII.GetString(readbuffer));
            }
            output.Flush();
            output.Close();
            output.Dispose();
            output = null;
        }
    }
}
