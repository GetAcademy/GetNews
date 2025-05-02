using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GetNews.Core.DomainModel
{
    public class VerificationResult
    {
        public bool IsSuccess { get; }
        public VerificationError? Error { get; }

        public VerificationResult(bool isSuccess, VerificationError? error) 
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static VerificationResult Success() => new(true, null);
        public static VerificationResult Fail(VerificationError error) => new(false, error);
    }
}
