using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AnalysisCat.Helper.Utils
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectHelper
    {
        /// <summary>
        /// 运行方法
        /// </summary>
        public static object RunMethod(string strClassName, string strMethodName, object[] parameters)
        {
            try
            {
                Type type = Type.GetType($"AnalysisCat.Helper.Asterix.{strClassName}");
                if (type != null)
                {
                    var vMethod = type.GetMethod(strMethodName);
                    if (vMethod != null)
                    {
                        return vMethod.Invoke(Activator.CreateInstance(type), parameters);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
