//  BuddyHub Universal Controller
//
//  Adapted by Husheng Deng, 2018
//  https://github.com/vomyrak/BuddyHub

//  Class adapted from David at https://www.codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server
//  This library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.

//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.

//  You should have received a copy of the GNU General Public License
//  along with this library.  If not, see <http://www.gnu.org/licenses/>.
//
//  All trademarks, service marks, trade names, product names are the property of their respective owners.


using System;
using System.Net;
using System.Threading;
using System.Text;

namespace UCProtocol
{
    public class WebServer
    {
        //Variables declared with "readonly" keyword is a runtime constant
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _respondMethod;
        
        //Func<in T, out TResult> specifies a method that takes parameter of type T and returns parameter of type TResult.
        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported) throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

            if (prefixes == null || prefixes.Length == 0) throw new ArgumentException("Prefixes");
            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _respondMethod = method ?? throw new ArgumentException("Method");
            
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
								ctx.Response.AppendHeader("Access-Control-Allow-Origin", "http://wsurop18-universal-controller.herokuapp.com");
                                
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch (Exception e){ Console.WriteLine(e); }
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
