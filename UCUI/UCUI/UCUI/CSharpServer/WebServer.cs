using System;
using System.Net;
using System.Threading;
using System.Text;

namespace CSharpServer
{
    class WebServer
    {
        //Variables declared with "readonly" keyword is a runtime constant
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _respondMethod;
        
        //Func<in T, out TResult> specifies a method that takes parameter of type T and returns parameter of type TResult.
        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported) throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            if (prefixes == null || prefixes.Length == 0) throw new ArgumentException("Prefixes");

            if (method == null) throw new ArgumentException("Method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _respondMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

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
                                string rstr = _respondMethod(ctx.Request);
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
