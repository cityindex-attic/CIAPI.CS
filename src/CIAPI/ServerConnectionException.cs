

using System;
using Salient.ReliableHttpClient;

namespace CIAPI.Rpc
{
    public class ThrottlingException:ReliableHttpException
    {
        public ThrottlingException(string message) : base(message)
        {
        }

        public ThrottlingException(string message, Exception ex) : base(message, ex)
        {
        }

        public ThrottlingException(Exception ex) : base(ex)
        {
        }

        public ThrottlingException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public ThrottlingException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class NoDataAvailableException:ReliableHttpException
    {
        public NoDataAvailableException(string message) : base(message)
        {
        }

        public NoDataAvailableException(string message, Exception ex) : base(message, ex)
        {
        }

        public NoDataAvailableException(Exception ex) : base(ex)
        {
        }

        public NoDataAvailableException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public NoDataAvailableException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidSessionException:ReliableHttpException
    {
        public InvalidSessionException(string message) : base(message)
        {
        }

        public InvalidSessionException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidSessionException(Exception ex) : base(ex)
        {
        }

        public InvalidSessionException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidSessionException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidCredentialsException:ReliableHttpException
    {
        public InvalidCredentialsException(string message) : base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidCredentialsException(Exception ex) : base(ex)
        {
        }

        public InvalidCredentialsException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidCredentialsException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidJsonRequestCaseFormatException:ReliableHttpException
    {
        public InvalidJsonRequestCaseFormatException(string message) : base(message)
        {
        }

        public InvalidJsonRequestCaseFormatException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidJsonRequestCaseFormatException(Exception ex) : base(ex)
        {
        }

        public InvalidJsonRequestCaseFormatException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidJsonRequestCaseFormatException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidJsonRequestException:ReliableHttpException
    {
        public InvalidJsonRequestException(string message) : base(message)
        {
        }

        public InvalidJsonRequestException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidJsonRequestException(Exception ex) : base(ex)
        {
        }

        public InvalidJsonRequestException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidJsonRequestException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidParameterValueException:ReliableHttpException
    {
        public InvalidParameterValueException(string message) : base(message)
        {
        }

        public InvalidParameterValueException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidParameterValueException(Exception ex) : base(ex)
        {
        }

        public InvalidParameterValueException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidParameterValueException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class ParameterMissingException:ReliableHttpException
    {
        
        public ParameterMissingException(string message) : base(message)
        {
        }

        public ParameterMissingException(string message, Exception ex) : base(message, ex)
        {
        }

        public ParameterMissingException(Exception ex) : base(ex)
        {
        }

        public ParameterMissingException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public ParameterMissingException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InvalidParameterTypeException:ReliableHttpException
    {
        public InvalidParameterTypeException(string message) : base(message)
        {
        }

        public InvalidParameterTypeException(string message, Exception ex) : base(message, ex)
        {
        }

        public InvalidParameterTypeException(Exception ex) : base(ex)
        {
        }

        public InvalidParameterTypeException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InvalidParameterTypeException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class InternalServerErrorException:ReliableHttpException
    {
        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception ex) : base(message, ex)
        {
        }

        public InternalServerErrorException(Exception ex) : base(ex)
        {
        }

        public InternalServerErrorException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public InternalServerErrorException(ReliableHttpException exception) : base(exception)
        {
        }
    }
    public class ForbiddenException:ReliableHttpException
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception ex) : base(message, ex)
        {
        }

        public ForbiddenException(Exception ex) : base(ex)
        {
        }

        public ForbiddenException(string message, ReliableHttpException exception) : base(message, exception)
        {
        }

        public ForbiddenException(ReliableHttpException exception) : base(exception)
        {
        }
    }

    public class ServerConnectionException : ReliableHttpException
    {
        public ServerConnectionException(string message, string responseText)
            : base(message)
        {
            ResponseText = responseText;
        }

        
    }
}