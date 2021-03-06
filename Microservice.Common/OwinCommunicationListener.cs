﻿
using System;
using System.Diagnostics;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using DH.Lending.Microservice.Common.Interfaces;

namespace DH.Lending.Microservice.Common
{
    public class OwinCommunicationListener : ICommunicationListener
    {
        /// <summary>
        /// OWIN server handle.
        /// </summary>
        private IDisposable serverHandle;

        private IOwinAppBuilder startup;
        private string publishAddress;
        private string listeningAddress;
        private string appRoot;
        private readonly ServiceInitializationParameters serviceInitializationParameters;

        public OwinCommunicationListener(IOwinAppBuilder startup, ServiceInitializationParameters serviceInitializationParameters)
            : this(null, startup, serviceInitializationParameters)
        {
        }

        public OwinCommunicationListener(string appRoot, IOwinAppBuilder startup, ServiceInitializationParameters serviceInitializationParameters)
        {
            this.startup = startup;
            this.appRoot = appRoot;
            this.serviceInitializationParameters = serviceInitializationParameters;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Initialize");

            EndpointResourceDescription serviceEndpoint = serviceInitializationParameters.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
            int port = serviceEndpoint.Port;

            if (serviceInitializationParameters is StatefulServiceInitializationParameters)
            {
                StatefulServiceInitializationParameters statefulInitParams = (StatefulServiceInitializationParameters)serviceInitializationParameters;

                this.listeningAddress = String.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}/{2}/{3}",
                    port,
                    statefulInitParams.PartitionId,
                    statefulInitParams.ReplicaId,
                    Guid.NewGuid());
            }
            else if (serviceInitializationParameters is StatelessServiceInitializationParameters)
            {
                this.listeningAddress = String.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}",
                    port,
                    String.IsNullOrWhiteSpace(this.appRoot)
                        ? String.Empty
                        : this.appRoot.TrimEnd('/') + '/');
            }
            else
            {
                throw new InvalidOperationException();
            }

            this.publishAddress = this.listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);

            Trace.WriteLine(String.Format("Opening on {0}", this.publishAddress));

            try
            {
                Trace.WriteLine(String.Format("Starting web server on {0}", this.listeningAddress));

                this.serverHandle = WebApp.Start(this.listeningAddress, appBuilder => this.startup.Configuration(appBuilder));

                return Task.FromResult(this.publishAddress);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);

                this.StopWebServer();

                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Close");

            this.StopWebServer();

            return Task.FromResult(true);
        }

        public void Abort()
        {
            Trace.WriteLine("Abort");

            this.StopWebServer();
        }

        private void StopWebServer()
        {
            if (this.serverHandle != null)
            {
                try
                {
                    this.serverHandle.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // no-op
                }
            }
        }
    }
}