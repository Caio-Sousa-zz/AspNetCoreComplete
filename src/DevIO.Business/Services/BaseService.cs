using DevIO.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                Notificar(erro.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            // Propagar error até camada de apresent~ção
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) 
            where TV : AbstractValidator<TE> 
            where TE : Entity
        {
            var validationResult = validacao.Validate(entidade);

            if(validationResult.IsValid) return true;

            Notificar(validationResult);

            return false;
        }
    }
}