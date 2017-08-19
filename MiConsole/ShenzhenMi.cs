using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using Ch.Elca.Iiop;
using hi.modMZ;
using hi.modSysInfo;
using omg.org.CosNaming;

namespace MiConsole
{
    public class ShenzhenMi
    {
        private ShenzhenMi()
        {
            Init();
        }

        private readonly NameComponent[] mzname = new NameComponent[] { new NameComponent("MZYL", "Service") };
        private readonly NameComponent[] sysname = new NameComponent[] { new NameComponent("SysInfo") };
        private NamingContext nameService;

        private IiopClientChannel channel = null;

        private intMZ mz = null;

        private intSys sys = null;

        private static ShenzhenMi curobj = null;

        public static ShenzhenMi Current
        {
            get { return curobj ?? (curobj = new ShenzhenMi()); }
        }
        private void Init()
        {
            // 建立并注册IIOP通道，与服务器corba进行通信 
            if (channel == null)
            {
                channel = new IiopClientChannel();
                ChannelServices.RegisterChannel(channel, false);
            }

            //本地corba初始化 
            //string nameserviceLoc = "corbaloc::61.144.253.81:10000/StandardNS/NameServer-POA/_root";
            //CorbaInit init = CorbaInit.GetInit();
            //ior文件中内容，也可从ior文件中读取 
            string ior = "IOR:000000000000002B49444C3A6F6D672E6F72672F436F734E616D696E672F4E616D696E67436F6E746578744578743A312E300000000000020000000000000074000102000000000E36312E3134342E3235332E38310027100000001F5374616E646172644E532F4E616D655365727665722D504F412F5F726F6F7400000000020000000000000008000000004A414300000000010000001C00000000000100010000000105010001000101090000000105010001000000010000002C0000000000000001000000010000001C00000000000100010000000105010001000101090000000105010001";
            //Ior iorobj = new Ior(ior);
            //IInternetIiopProfile iiopProf = (IInternetIiopProfile)iorobj.Profiles[0];

            //根据ior字符串获取对方命名服务 
            nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), ior);
            //RmiIiopInit init = new RmiIiopInit("61.144.253.81", 10000);
            //NamingContext nameService = (NamingContext)init.GetService("StandardNS/NameServer-POA/_root");
            //NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), nameserviceLoc);

        }
        public intMZ MZService
        {
            get
            {
                if (mz == null)
                {
                    //根据对象绑定的服务名获取远程对象 
                    mz = (intMZ) nameService.resolve(mzname);
                }
                return mz;
            }
        }
        public intSys SysService
        {
            get
            {
                if (sys == null)
                {
                    //根据对象绑定的服务名获取远程对象 
                    
                    sys = (intSys) nameService.resolve(sysname);
                }
                return sys;
            }
        }

        private void Destroy()
        {
            mz = null;
            sys = null;
            nameService = null;
            ChannelServices.UnregisterChannel(channel);
            channel = null;
        }

        public static void CleanUp()
        {
            if (curobj != null)
            {
                curobj.Destroy();
                curobj = null;
            }
        }
        #region Common
        //string类型转为short[] 
        public static short[] TransfromSa(string strArray)
        {
            short[] shortArray = new short[strArray.Length];
            char[] chars = strArray.ToCharArray();
            for (int i = 0; i < strArray.Length; i++)
            {
                shortArray[i] = TransCharToShort(chars[i]);
            }
            return shortArray;
        }

        public static short TransCharToShort(char c)
        {
            char[] t = new char[1];
            t[0] = c;
            string strTemp = new string(t);
            byte[] cc = System.Text.Encoding.Default.GetBytes(strTemp);
            if (cc.Length == 1)
            {
                return (short)(cc[0] < 0 ? cc[0] + 256 : cc[0]);
            }
            int v1 = cc[0] < 0 ? cc[0] + 256 : cc[0];
            int v2 = cc[1] < 0 ? cc[1] + 256 : cc[1];
            return (short)((v1 << 8) + v2);
        }
        //short[] 转为string类型 
        public static string TransformAs(short[] sArray)
        {
            char[] charArray = new char[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                charArray[i] = TransShortToChar(sArray[i]);
            }
            return new String(charArray);
        }
        private static char TransShortToChar(short sTemp)
        {
            if (sTemp >= 0)
            {
                return (char)sTemp;
            }

            byte[] b = new byte[2];
            b[0] = (byte)(sTemp >> 8);
            b[1] = (byte)sTemp;
            string strTemp = System.Text.Encoding.Default.GetString(b);
            char[] charArrTemp = strTemp.ToCharArray();
            return charArrTemp[0];
        }
        #endregion
    }
}
