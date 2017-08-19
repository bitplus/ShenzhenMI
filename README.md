# 深圳医保接口示例

## A sample for Shenzhen medical insurance CORBA interface

本示例参考了文章 http://wzhiju.iteye.com/blog/1183519 提供的方法，使用IIOP.NET调用CORBA接口。
使用过程中遇到了一些坑，都填上了。

例如：

> org.omg.CORBA.MARSHAL异常

原因是使用了错误的IDL文件生成DLL，目前应该用 `hi-非少儿.idl` 这个文件。

又例如：

> CORBA system exception : omg.org.CORBA.TRANSIENT [Unable to connect to target.] , completed: Completed_No minor: 4000.

具体参考https://sourceforge.net/p/iiop-net/discussion/274082/thread/cc90ddcc/?limit=25

医保接口的文档提到了需要设置ior_proxy_host，但是貌似IIOP.NET不支持，只能动手改了。

改动如下：

> IIOPChannel\IOR.cs
> 484行开始

```c#
public string HostName
{
    get
    {
        //Read IOR proxy from config file
        string iorProxyHost = ConfigurationManager.AppSettings["ior_proxy_host"];
        if (!string.IsNullOrEmpty(iorProxyHost))
        {
            return ConfigurationManager.AppSettings["ior_proxy_host"];
        }
        return m_hostName;
    }
}
```

这样就可以在App.config(或web.config)配置ior_proxy_host了。

---------------------

示例使用方法：

1. 用Visual Studio 2013或以上打开MiConsole.sln，全部生成运行即可。
2. Scripts目录有个简单的批处理，用来通过接口IDL文件生成DLL。