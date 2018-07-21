﻿using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CSharpServer
{
    class WebServer
    {
        //Variables declared with "readonly" keyword is a runtime constant
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, List<DeviceInfo>, string> _respondMethod;
        private List<DeviceInfo> ConnectedDeviceList;

        //Func<in T, out TResult> specifies a method that takes parameter of type T and returns parameter of type TResult.
        public WebServer(List<DeviceInfo> ConnectedDeviceList, string[] prefixes, Func<HttpListenerRequest, List<DeviceInfo>, string> method)
        {
            if (!HttpListener.IsSupported) throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            if (prefixes == null || prefixes.Length == 0) throw new ArgumentException("Prefixes");

            if (method == null) throw new ArgumentException("Method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _respondMethod = method;
            _listener.Start();
            this.ConnectedDeviceList = ConnectedDeviceList;
        }

        public WebServer(List<DeviceInfo> connectedDeviceList, Func<HttpListenerRequest, List<DeviceInfo>, string> method, params string[] prefixes)
            : this(connectedDeviceList, prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _respondMethod(ctx.Request, ConnectedDeviceList);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
