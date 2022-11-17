using Microsoft.Extensions.Configuration.Json;

namespace OilSystem.Configuration
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration? Configuration { get; set; }
        static string? ContentPath { get; set; }

        public Appsettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //1.获取当前环境 不能直接使用 Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")进行获取，开发环境不会报错，发布以后会报错，需要发布后再配置，所以不建议使用这种方法获取
            //var env = Environment.GetEnvironmentVariables();
            //2.从env中获取环境
            //var envName = env["ASPNETCORE_ENVIRONMENT"];
            //string Path = $"appsettings.{envName}.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }

        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                    return Configuration[string.Join(":", sections)];
#pragma warning restore CS8602 // 解引用可能出现空引用。
                }
            }
            catch (Exception)
            {

            }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
    }

}
