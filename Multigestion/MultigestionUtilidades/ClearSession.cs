using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace MultigestionUtilidades
{
    public class ClearSession : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Session["Date"] = null;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
