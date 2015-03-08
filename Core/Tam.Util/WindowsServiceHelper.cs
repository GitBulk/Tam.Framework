using System;
using System.ServiceProcess;

namespace Tam.Util
{
    internal class WindowsServiceHelper
    {
        /// <summary>
        /// Get Window service
        /// </summary>
        /// <param name="serviceName">Service name</param>
        public static ServiceController GetWindowsService(string serviceName)
        {
            try
            {
                System.ServiceProcess.ServiceController[] services;
                services = System.ServiceProcess.ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Equals(serviceName))
                    {
                        //return new ServiceController(serviceName);
                        return service;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Stop windows service
        /// </summary>
        /// <param name="serviceToStop">Service to stop</param>
        public static void StopWindowsService(ServiceController serviceToStop)
        {
            try
            {
                if (serviceToStop.Status == ServiceControllerStatus.Stopped)
                {
                    return;
                }
                if (serviceToStop.CanStop)
                {
                    //get an array of dependent services, loop through the array and
                    //prompt the user to stop all dependent services.
                    ServiceController[] dependentServices = serviceToStop.DependentServices;
                    //if the length of the array is greater than or equal to 1.
                    if (dependentServices.Length >= 1)
                    {
                        foreach (ServiceController DependentService in dependentServices)
                        {
                            //make sure the dependent service is not already stopped.
                            if (DependentService.Status.ToString() != "Stopped")
                            {
                                // not checking at this point whether the dependent service can be stopped.
                                // developer may want to include this check to avoid exception.
                                DependentService.Stop();
                                DependentService.WaitForStatus(ServiceControllerStatus.Stopped);
                            }
                        }
                    }

                    if (serviceToStop.Status.ToString().Equals("Running") || serviceToStop.Status.ToString().Equals("Paused"))
                    {
                        serviceToStop.Stop();
                    }
                    serviceToStop.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void StartWindowsService(ServiceController serviceToStart)
        {
            try
            {
                //check the status of the service
                if (serviceToStart.Status.ToString().Equals("Paused"))
                {
                    serviceToStart.Continue();
                }
                else if (serviceToStart.Status.ToString().Equals("Stopped"))
                {
                    //get an array of services this service depends upon, loop through
                    //the array and prompt the user to start all required services.
                    ServiceController[] ParentServices = serviceToStart.ServicesDependedOn;

                    //if the length of the array is greater than or equal to 1.
                    if (ParentServices.Length >= 1)
                    {
                        foreach (ServiceController ParentService in ParentServices)
                        {
                            //make sure the parent service is running or at least paused.
                            if (!ParentService.Status.ToString().Equals("Running") || !ParentService.Status.ToString().Equals("Paused"))
                            {
                                ParentService.Start();
                                ParentService.WaitForStatus(ServiceControllerStatus.Running);
                            }
                        }
                    }
                    serviceToStart.Start();
                    serviceToStart.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}