using System;
using System.Management;

class Program
{
    static bool deviceConnected = false;
    static bool deviceDisconnected = false;

    static void Main()
    {
        var watcher = new ManagementEventWatcher();

        var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");
        watcher.Query = query;
       
        watcher.EventArrived += (sender, e) =>
        {
            var eventType = Convert.ToInt32(e.NewEvent.Properties["EventType"].Value);

            if (eventType == 2) 
            {
                if (!deviceConnected)
                {
                    Console.WriteLine("Podłączono urządzenie USB");

                    deviceConnected = true;

                    deviceDisconnected = false;
                }
            }
            else if (eventType == 3) 
            {
                if (!deviceDisconnected)
                {
                    deviceConnected = false;
                   
                    Console.WriteLine("Odłączono urządzenie USB");
                    
                    deviceDisconnected = true;
                }
            }
        };
        watcher.Start();
        
        Console.WriteLine("Oczekiwanie na podłączenie urządzenia USB. Naciśnij dowolny klawisz, aby zakończyć.");
        
        Console.ReadKey();
       
        watcher.Stop();
    }
}
