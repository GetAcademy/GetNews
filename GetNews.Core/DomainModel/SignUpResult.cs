//  

//  Importing necessary namespaces
using GetNews.Core.DomainModel;

public class SignUpResult
{
    public SignUpError? Error { get; }
    public bool IsSuccess => Error == null;
    public Subscription? Subscription { get; }
    public Email? Email { get; }

    private SignUpResult(SignUpError error)
    {
        Error = error;
    }

    private SignUpResult(Subscription? subscription = null, Email? email = null)
    {
        Subscription = subscription;
        Email = email;
    }

    public static SignUpResult Ok(Subscription subscription, Email email)
        => new SignUpResult(subscription, email);

    public static SignUpResult Fail(SignUpError signUpError)
        => new SignUpResult(signUpError);
}