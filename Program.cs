using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC_Firmware_Update
{
    static class Program
    {
        public static TcpClient client;
        public static RotaAdvanceDeviceInfo rotaAdvanceDeviceInfo;
        public static RotaDeviceInfo rotaDeviceInfo;
        public static Boolean isIPAccessable = false;
        public static Boolean isConnected = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            rotaAdvanceDeviceInfo = new RotaAdvanceDeviceInfo();
            rotaDeviceInfo = rotaAdvanceDeviceInfo.DeviceInfo;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }



        public static Boolean Connect(String server, Int32 port)
        {
            if (!isIPAccessable)
            {
                return false;
            }
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                client = new TcpClient(server, port);
                isConnected = client.Connected;
                return isConnected;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return false;
        }


        public static void Disconnect()
        {
            if (!isConnected)
            {
                return;
            }
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.

                client.Close();
                isConnected = false;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }


        public static String CheckAccess(String ipAddr)
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            PingReply reply = p.Send(ipAddr);
            String statusMsg = "Success";
            if (reply.Status.ToString().Equals(statusMsg))
            {
                isIPAccessable = true;
            }
            else
            {
                isIPAccessable = false;
                statusMsg = "Check your IP address !!!";
            }
            return statusMsg;
        }


        public static Boolean SendData(byte[] data)
        {
            if (client == null) {
                return false;
            }
            if (!client.Connected) {
                return false;
            }
            try
            {
                NetworkStream stream = client.GetStream();

                
                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                return true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e) {
                Console.WriteLine("Exception: {0}", e);
            }
            return false;
        }


        public static byte[] ReceiveData()
        {
            if (client == null)
            {
                return null;
            }
            // Buffer to store the response bytes.
            byte[] data = new byte[1400];
            if (!client.Connected)
            {
                return null;
            }
            try
            {
                NetworkStream stream = client.GetStream();

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);                
                return data;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            return data;
        }


    }
}
