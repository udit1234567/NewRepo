using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIA.Life.Models.MQModel
{
    public class QueueModel
    {
        public class MQRCText
        {
            static int idx = 0;
            static ArrayList mesText = new ArrayList();
            public string getMQRCText(int MsgNo)
            {
                idx = 0;
                if (MsgNo != 0)
                {
                    idx = MsgNo - 2000;
                }
                try
                {
                    return mesText[idx].ToString();
                }
                catch (Exception)
                {
                    // Handle exception
                    return "NOTFOUND";
                }
            }

        }
    }
}
