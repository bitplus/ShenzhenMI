using System;
using hi.modMZ;
using hi.modSysInfo;

namespace MiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Demo();
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: " + e);
            }
            Console.ReadLine();
        }

        public static void Demo()
        {
            string yljgbm = "XX100";
            string ylzh = "%ABCDEFGHIJK?;0123456789?";
            string pass = "";
            //string zzyybm = "*****";
            string czybm = "XX101";
            string czy = "某某某";
            short[] czyxm = ShenzhenMi.TransfromSa(czy);

            hi.GRXX hr;
            hi.modSysInfo.XXDetail[] msg;
            intMZ mz = ShenzhenMi.Current.MZService;
            
            Console.WriteLine();

            //测试方法             
            short[] res = mz.GetGRJBXX(yljgbm, ylzh, pass, czybm, czyxm, out hr);
            Console.WriteLine(ShenzhenMi.TransformAs(res));
            Console.WriteLine(hr.ACCOUNT);
            Console.WriteLine(hr.YLZH);
            Console.WriteLine(hr.XB);
            Console.WriteLine("finished!");

            ShenzhenMi.CleanUp();
            //Thread.Sleep(30000);
            intSys sys = ShenzhenMi.Current.SysService;
            string strres;
            short[] res1 = sys.GetState(out strres);
            Console.WriteLine(ShenzhenMi.TransformAs(res1));
            Console.WriteLine(strres);
            Console.ReadLine();
            res1 = sys.GetMassage(out msg);
            Console.WriteLine(ShenzhenMi.TransformAs(res1));
            Console.WriteLine(msg.Length);
            Console.ReadLine();
            foreach (var detail in msg)
            {
                Console.WriteLine(string.Format("{0},{1},{2}", ShenzhenMi.TransformAs(detail.TZBT), ShenzhenMi.TransformAs(detail.TZNR), detail.XXBM));
            }
            ShenzhenMi.CleanUp();
        }

    }
}
