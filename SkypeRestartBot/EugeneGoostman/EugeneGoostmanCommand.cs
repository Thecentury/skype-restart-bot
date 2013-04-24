using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot.EugeneGoostman
{
    public sealed class EugeneGoostmanCommand : ICommand
    {
        private readonly string _url;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ChatMessage _message;
        private readonly ISkypeMessenger _skype;

        public EugeneGoostmanCommand(ChatMessage message, string url, ISkypeMessenger skype)
        {
            _message = message;
            _url = url;
            _skype = skype;
        }

        public void Execute(ServiceInfo target, string alias)
        {
            Task.Factory.StartNew(() => ExecuteCore(alias));
        }

        private void ExecuteCore(string alias)
        {
            try
            {
                WebClient web = new WebClient();

                string request = _url + WebUtility.HtmlEncode(alias);

                string response = web.DownloadString(request);

                _skype.Reply(_message, response);
            }
            catch (Exception exc)
            {
                _logger.Error("{0}.Execute( '{1}' ) failed: {2}", GetType().Name, alias, exc);
            }
        }

        public string Verb
        {
            get { return String.Empty; }
        }
    }
}
