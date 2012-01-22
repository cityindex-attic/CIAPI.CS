using System;
using System.Collections.Generic;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class LogAdapter : ILog
    {
        private readonly Dictionary<string, ReflectedMethodInfo> _methods;
        private readonly Type _innerType;
        private readonly object _inner;
        public bool ThrowIfMethodMissing = true;
        public bool SwallowInnerExceptions = false;
        public LogAdapter(object inner)
        {
            _inner = inner;
            _innerType = _inner.GetType();
            _methods = new Dictionary<string, ReflectedMethodInfo>();

            AddMethodInfo("Debug", new[] { typeof(object) });
            AddMethodInfo("Debug", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("DebugFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("DebugFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            



            AddMethodInfo("Error", new[] { typeof(object) });
            AddMethodInfo("Error", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("ErrorFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("ErrorFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            

            AddMethodInfo("Fatal", new[] { typeof(object) });
            AddMethodInfo("Fatal", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("FatalFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("FatalFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            

            AddMethodInfo("Info", new[] { typeof(object) });
            AddMethodInfo("Info", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("InfoFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("InfoFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            

            AddMethodInfo("Trace", new[] { typeof(object) });
            AddMethodInfo("Trace", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("TraceFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("TraceFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            

            AddMethodInfo("Warn", new[] { typeof(object) });
            AddMethodInfo("Warn", new[] { typeof(object), typeof(Exception) });

            AddMethodInfo("WarnFormat", new[] { typeof(string), typeof(object[]) });
            
            AddMethodInfo("WarnFormat", new[] { typeof(string), typeof(Exception), typeof(object[]) });
            

        }

        

        private void AddMethodInfo(string methodName, params Type[] ptypes)
        {
            var mi = ReflectedMethodInfo.Create(_innerType, methodName, ptypes);

            if (mi != null)
            {
            
                _methods.Add(mi.ToString(), mi);
            }
        }

        private void ExecuteLoggingAction(string methodSignature, params object[] parameters)
        {


            ReflectedMethodInfo methodInfo = null;

            if (_methods.ContainsKey(methodSignature))
            {
                methodInfo = _methods[methodSignature];
            }
            else
            {
                if(ThrowIfMethodMissing)
                {
                    throw new Exception("Log method missing: " + methodSignature);
                }
            }

            if (methodInfo != null)
            {
                try
                {
                    methodInfo.Method.Invoke(_inner, parameters);
                }
                catch
                {
                    if (!SwallowInnerExceptions)
                    {
                        throw;
                    }
                }
            }
            
        }

        #region Debug

        public bool IsTraceEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDebugEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsErrorEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFatalEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInfoEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsWarnEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Debug(object message)
        {
            ExecuteLoggingAction("Debug Object", message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            ExecuteLoggingAction("Debug Object, Exception", message, exception);
        }

        #endregion


        #region DebugFormat
        public virtual void DebugFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("DebugFormat String, Object[]",format, args);

        }

        

        public virtual void DebugFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("DebugFormat String, Exception, Object[]",format, exception, args);
        }

        #endregion

        #region Error
        public virtual void Error(object message)
        {
            ExecuteLoggingAction("Error Object", message);
        }

        public virtual void Error(object message, Exception exception)
        {
            ExecuteLoggingAction("Error Object, Exception", message, exception);
        }
        #endregion

        #region ErrorFormat
        public virtual void ErrorFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("ErrorFormat String, Object[]", format, args);
        }


        public virtual void ErrorFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("ErrorFormat String, Exception, Object[]", format, exception, args);
        }

        #endregion

        #region Fatal
        public virtual void Fatal(object message)
        {
            ExecuteLoggingAction("Fatal Object", message);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            ExecuteLoggingAction("Fatal Object, Exception", message, exception);
        }
        #endregion

        #region FatalFormat
        public virtual void FatalFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("FatalFormat String, Object[]", format, args);
        }


        public virtual void FatalFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("FatalFormat String, Exception, Object[]", format, exception, args);
        }

        #endregion

        #region Info
        public virtual void Info(object message)
        {
            ExecuteLoggingAction("Info Object", message);
        }

        public virtual void Info(object message, Exception exception)
        {
            ExecuteLoggingAction("Info Object, Exception", message, exception);
        }
        #endregion

        #region InfoFormat
        public virtual void InfoFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("InfoFormat String, Object[]", format, args);
        }


        public virtual void InfoFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("InfoFormat String, Exception, Object[]",format, exception, args);
        }

        #endregion

        #region Trace
        public virtual void Trace(object message)
        {
            ExecuteLoggingAction("Trace Object", message);
        }

        public virtual void Trace(object message, Exception exception)
        {
            ExecuteLoggingAction("Trace Object, Exception", message, exception);
        }
        #endregion

        #region TraceFormat
        public virtual void TraceFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("TraceFormat String, Object[]", format, args);
        }

        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("TraceFormat String, Exception, Object[]", format, exception, args);
        }

        #endregion

        #region Warn
        public virtual void Warn(object message)
        {
            ExecuteLoggingAction("Warn Object", message);
        }

        public virtual void Warn(object message, Exception exception)
        {
            ExecuteLoggingAction("Warn Object, Exception", message, exception);
        }
        #endregion

        #region WarnFormat
        public virtual void WarnFormat(string format, params object[] args)
        {
            ExecuteLoggingAction("WarnFormat String, Object[]", format, args);
        }


        public virtual void WarnFormat(string format, Exception exception, params object[] args)
        {
            ExecuteLoggingAction("WarnFormat String, Exception, Object[]", format, exception, args);
        }


        #endregion
 
    }
}