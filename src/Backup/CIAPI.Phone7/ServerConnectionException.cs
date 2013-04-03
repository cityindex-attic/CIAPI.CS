

using System;
using System.Collections.Generic;
using System.Text;
using Salient.ReliableHttpClient;

// ReSharper disable CheckNamespace
namespace CIAPI.Rpc
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AggregateException:ReliableHttpException
    {
        private List<Exception> _exceptions = new List<Exception>();
        /// <summary>
        /// 
        /// </summary>
        public List<Exception> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AggregateException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public AggregateException(string message, Exception ex) : base(message, ex)
        {
            _exceptions.Add(ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public AggregateException(Exception ex) : base(ex)
        {
            _exceptions.Add(ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public AggregateException(string message, ReliableHttpException ex)
            : base(message, ex)
        {
            _exceptions.Add(ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public AggregateException(ReliableHttpException ex)
            : base(ex)
        {
            _exceptions.Add(ex);
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
        public override string ToString()
        {
            var sb= new StringBuilder();
            foreach (var ex in _exceptions)
            {
                sb.AppendLine(ex.Message);

            }
            sb.AppendLine(base.ToString());
            return sb.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ThrottlingException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ThrottlingException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ThrottlingException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public ThrottlingException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ThrottlingException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ThrottlingException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NoDataAvailableException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NoDataAvailableException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public NoDataAvailableException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public NoDataAvailableException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public NoDataAvailableException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public NoDataAvailableException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidSessionException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidSessionException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidSessionException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidSessionException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidSessionException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidSessionException(ReliableHttpException exception) : base(exception)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidJsonRequestCaseFormatException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidJsonRequestCaseFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidJsonRequestCaseFormatException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidJsonRequestCaseFormatException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidJsonRequestCaseFormatException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidJsonRequestCaseFormatException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidJsonRequestException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidJsonRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidJsonRequestException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidJsonRequestException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidJsonRequestException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidJsonRequestException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidParameterValueException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidParameterValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidParameterValueException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidParameterValueException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidParameterValueException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidParameterValueException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ParameterMissingException:ReliableHttpException
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ParameterMissingException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ParameterMissingException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public ParameterMissingException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ParameterMissingException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ParameterMissingException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidParameterTypeException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidParameterTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidParameterTypeException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidParameterTypeException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidParameterTypeException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidParameterTypeException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InternalServerErrorException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InternalServerErrorException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InternalServerErrorException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InternalServerErrorException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InternalServerErrorException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InternalServerErrorException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ForbiddenException:ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ForbiddenException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ForbiddenException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public ForbiddenException(Exception ex) : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ForbiddenException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public ForbiddenException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ServerConnectionException : ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="responseText"></param>
        public ServerConnectionException(string message, string responseText)
            : base(message)
        {
            ResponseText = responseText;
        }

        
    }
}