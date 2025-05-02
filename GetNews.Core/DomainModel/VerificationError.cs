using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNews.Core.DomainModel
{
    public enum VerificationError 
    {
        SubscriptionNotFound,
        InvalidCodeFormat,
        VerificationCodeMissing,
        CodeMismatch,
        EmailMismatch
    }

}